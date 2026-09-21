# Travel database ERD

The model uses one-to-many relationships for countries/cities, cities/attractions, attractions/reviews, and users/reviews. Attractions and categories use an EF Core many-to-many relationship through the generated `AttractionCategories` junction table.

```mermaid
erDiagram
    COUNTRY ||--o{ CITY : contains
    CITY ||--o{ ATTRACTION : contains
    ATTRACTION }o--o{ CATEGORY : classified_as
    ATTRACTION ||--o{ REVIEW : receives
    USER ||--o{ REVIEW : writes

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
        datetime2 CreatedAt
    }
    CATEGORY {
        uniqueidentifier CategoryId PK
        varchar Name
    }
    REVIEW {
        uniqueidentifier ReviewId PK
        uniqueidentifier AttractionId FK
        uniqueidentifier UserId FK
        varchar CommentText
        tinyint Score
        datetime2 CreatedAt
    }
    USER {
        uniqueidentifier UserId PK
        varchar Username
        varchar Email
        datetime2 CreatedAt
    }
```
