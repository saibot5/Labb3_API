# Labb3_API

## Hämta alla personer i systemet:
/users

## Hämta alla intressen som är kopplade till en specifik person: 
/interest/{userId}

## Hämta alla länkar som är kopplade till en specifik person:
/links/{userId}

## Koppla en person till ett nytt intresse: 
/user/{uid}/interest/{iId}

## Lägga in nya länkar för en specifik person och ett specifikt intresse:
(vet inte om den följer med i git men för mig så  hamnade en parameter för url string i swagger som jag inte får bort. fyll med vad som helst då den inte används men är required.)  
  
/users/{uId}/Interest/{id}/link  
  
body:  
{  
  "linkId": 0,  
  "website": "string"  
}
