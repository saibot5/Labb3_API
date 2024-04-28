
using Labb3_API.Data;
using Labb3_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Labb3_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            //Get all Users
            app.MapGet("/users", async (ApplicationDbContext context) =>
            {
                var users = await context.Users.Include(i => i.Interests).ToListAsync();




                if (users == null || !users.Any())
                {
                    return Results.NotFound("no users found");
                }

                return Results.Ok(users);
            });

            //create User
            app.MapPost("/users", async (User user, ApplicationDbContext context) =>
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return Results.Created($"/users/{user.UserId}", user);
            });

            //Get interests by Userid
            app.MapGet("/intrest/{userId:int}", async (int userId, ApplicationDbContext context) =>
            {
                var user = await context.Users.FindAsync(userId);
                List<Interest> interests = await context.Interests.Where(u => u.Users.Contains(user)).Include(l =>l.Links.Where(u => u.User == user)).ToListAsync();
                
                if (user == null)
                {
                    return Results.NotFound($"No user with id:{userId} found");
                }

                UserInterestViewModel viewModel = new UserInterestViewModel
                {
                    UserName = user.UserName,
                    Interests = interests

                };

                return Results.Ok(viewModel);
            });

            //Get Lnks by userid
            app.MapGet("/links/{userId:int}", async (int userId, ApplicationDbContext context) =>
            {
                var user = await context.Users.FindAsync(userId);
                List<Link> links = await context.Links.Where(u => u.User == user).ToListAsync();

                if (user == null)
                {
                    return Results.NotFound($"No user with id:{userId} found");
                }

         

                return Results.Ok(links);
            });


            ////Edit User
            //app.MapPut("/users/{id:int}", async (int id, User updatedUser, ApplicationDbContext context) =>
            //{
            //    var user = await context.Users.FindAsync(id);

            //    if (user == null)
            //    {
            //        return Results.NotFound($"No user with id:{id} found");
            //    };
            //    user.UserName = updatedUser.UserName;
            //    user.phonenumber = updatedUser.phonenumber;
            //    await context.SaveChangesAsync();
            //    return Results.Ok(user);
            //});

            ////delete user
            //app.MapDelete("/users/{id:int}", async (int id, ApplicationDbContext context) =>
            //{
            //    var user = await context.Users.FindAsync(id);

            //    if (user == null)
            //    {
            //        return Results.NotFound($"No user with id:{id} found");
            //    }
            //    context.Users.Remove(user);
            //    await context.SaveChangesAsync();
            //    return Results.Ok($"user with id: {id} deleted");
            //});

            //add interest to User
            app.MapPut("/users/{uId:int}/interest/{iId:int}", async (int uId, int iId, ApplicationDbContext context) =>
            {
                var user = await context.Users.FindAsync(uId);
                var interest = await context.Interests.FindAsync(iId);

                if (user == null)
                {
                    return Results.NotFound($"No user with id:{uId} found");
                }
                if (interest == null)
                {
                    return Results.NotFound($"No interest with id:{iId} found");
                };


                user.Interests.Add(interest);
                await context.SaveChangesAsync();
                return Results.Ok(user);

            });

            //add Link to User interest
            app.MapPut("/users/{uId:int}/interest/{iId:int}/link", async (int uId, int iId,string url,Link link, ApplicationDbContext context) =>
            {
                var user = await context.Users.FindAsync(uId);
                var interest = await context.Interests.FindAsync(iId);
               


                if (user == null)
                {
                    return Results.NotFound($"No user with id:{uId} found");
                }
                if (interest == null)
                {
                    return Results.NotFound($"No interest with id:{iId} found");
                };

                link.User = user;
                link.interest = interest;

                interest.Links.Add(link);
                
                await context.SaveChangesAsync();
                return Results.Ok(link);

            });
            //////// INTEREST ///////////

            //Get all interests
            app.MapGet("/interests", async (ApplicationDbContext context) =>
            {
                var interests = await context.Interests.ToListAsync();

                if (interests == null || !interests.Any())
                {
                    return Results.NotFound("no interest found");
                }
                return Results.Ok(interests);
            });

            //create interest
            app.MapPost("/interests", async (Interest interest, ApplicationDbContext context) =>
            {
                context.Interests.Add(interest);
                await context.SaveChangesAsync();
                return Results.Created($"/interest/{interest.InterestId}", interest);
            });

            ////Get an interest by id
            //app.MapGet("/interests/{id:int}", async (int id, ApplicationDbContext context) =>
            //{
            //    var interest = await context.Interests.FindAsync(id);

            //    if (interest == null)
            //    {
            //        return Results.NotFound($"No interest with id:{id} found");
            //    }
            //    return Results.Ok(interest);
            //});

            ////Edit interest
            //app.MapPut("/interests/{id:int}", async (int id, Interest updatedInterest, ApplicationDbContext context) =>
            //{
            //    var interest = await context.Interests.FindAsync(id);

            //    if (interest == null)
            //    {
            //        return Results.NotFound($"No interest with id:{id} found");
            //    };
            //    interest.Title = updatedInterest.Title;
            //    interest.Description = updatedInterest.Description;
            //    await context.SaveChangesAsync();
            //    return Results.Ok(interest);
            //});

            ////delete interest
            //app.MapDelete("/interests/{id:int}", async (int id, ApplicationDbContext context) =>
            //{
            //    var interest = await context.Interests.FindAsync(id);

            //    if (interest == null)
            //    {
            //        return Results.NotFound($"No interest with id:{id} found");
            //    }
            //    context.Interests.Remove(interest);
            //    await context.SaveChangesAsync();
            //    return Results.Ok($"interest with id: {id} deleted");
            //});

            ///////// Links //////////

            ////Get all link
            //app.MapGet("/links", async (ApplicationDbContext context) =>
            //{
            //    var links = await context.Links.ToListAsync();

            //    if (links == null || !links.Any())
            //    {
            //        return Results.NotFound("no links found");
            //    }
            //    return Results.Ok(links);
            //});

            ////create link
            //app.MapPost("/link", async (Link link, ApplicationDbContext context) =>
            //{
            //    context.Links.Add(link);
            //    await context.SaveChangesAsync();
            //    return Results.Created($"/link/{link.LinkId}", link);
            //});

            ////Get an link by id
            //app.MapGet("/link/{id:int}", async (int id, ApplicationDbContext context) =>
            //{
            //    var link = await context.Links.FindAsync(id);

            //    if (link == null)
            //    {
            //        return Results.NotFound($"No link with id:{id} found");
            //    }
            //    return Results.Ok(link);
            //});

            ////Edit link
            //app.MapPut("/links/{id:int}", async (int id, Link updatedLink, ApplicationDbContext context) =>
            //{
            //    var link = await context.Links.FindAsync(id);

            //    if (link == null)
            //    {
            //        return Results.NotFound($"No interest with id:{id} found");
            //    };
            //    link.website = updatedLink.website;
            //    link.User = updatedLink.User;
            //    link.interest = updatedLink.interest;
            //    await context.SaveChangesAsync();
            //    return Results.Ok(link);
            //});

            ////delete link
            //app.MapDelete("/links/{id:int}", async (int id, ApplicationDbContext context) =>
            //{
            //    var link = await context.Links.FindAsync(id);

            //    if (link == null)
            //    {
            //        return Results.NotFound($"No link with id:{id} found");
            //    }
            //    context.Links.Remove(link);
            //    await context.SaveChangesAsync();
            //    return Results.Ok($"link with id: {id} deleted");
            //});



            app.Run();
        }
    }
}
