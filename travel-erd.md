# Travel ERD TOMAS RUNNVIK - Projekt.
 Då jag utgått ifrån vår förra kurs med databasprojektet har jag följt samma struktur. 
Modeller: `Country`, `City`, `Attraction`, `Category`, `Review` och `User`.

 Ett land kan ha flera cities 1-* en till många.

 En stad kan ha flera attractions 1-* en till många.

 En attraktion kan ha flera reviews 1-* en till många.

 En användare kan skriva flera reviews 1-* en till många
 .
 En attraktion kan tillhöra flera categories *-* många till många.

 Använder uniqueidentifier/Guid som primärnyckel för alla tabeller för att säkerställa globala unika identifierare.

 Attractions och categories har en många-till-många-relation via en junction table.


```mermaid
erDiagram
    COUNTRY ||--o{ CITY : contains
    CITY ||--o{ ATTRACTION : contains
    
    ATTRACTION ||--o{ REVIEW : receives
    USER ||--o{ REVIEW : writes
    ATTRACTION ||--o{ ATTRACTIONCATEGORIES : contains
    CATEGORY ||--o{ ATTRACTIONCATEGORIES : contains

    COUNTRY {
        uniqueidentifier CountryId PK
        varchar Name
    }
    CITY {
        uniqueidentifier CityId PK
        uniqueidentifier CountryId FK
        varchar Name
    }
    ATTRACTION {
        uniqueidentifier AttractionId PK
        uniqueidentifier CityId FK
        varchar Name
        varchar Description
        varchar Address
        datetime2 CreatedAt
    }
    CATEGORY {
        uniqueidentifier CategoryId PK
        varchar Name
    }
    ATTRACTIONCATEGORIES {
        uniqueidentifier AttractionId PK, FK
        uniqueidentifier CategoryId PK, FK
    }
    REVIEW {
        uniqueidentifier ReviewId PK
        uniqueidentifier AttractionId FK
        uniqueidentifier UserId FK
        varchar CommentText
        int Score
        datetime2 CreatedAt
    }
    USER {
        uniqueidentifier UserId PK
        varchar Username
        varchar Email
        datetime2 CreatedAt
    }
```
