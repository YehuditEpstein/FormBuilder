# FormBuilder – בניית טפסים וניהול אבני דרך

אב-טיפוס (PoC) למערכת ליצירת טפסים ארגוניים והגדרת מסלול אישורים דינמי (אבני
דרך) עבור כל טופס — כולל שמירה, שליפת רשימה ושליפת טופס בודד.

**Stack:** Angular 18 (standalone components, Reactive Forms) · ASP.NET Core 8
Web API · EF Core + SQLite.

## ארכיטקטורה

```
backend/                 פתרון .NET, מחולק לשכבות (Clean Architecture)
  src/
    FormBuilder.Domain          ישויות עסקיות טהורות, ללא תלות בשום דבר חיצוני
    FormBuilder.Application     Use-cases, DTOs, ולידציה, ממשקים (Interfaces)
    FormBuilder.Infrastructure  EF Core, מימוש ה-Repository, גישה ל-DB
    FormBuilder.Api             Controllers, Middleware, Composition Root, Swagger
  tests/
    FormBuilder.UnitTests       בדיקות יחידה לשכבת ה-Application (xUnit + Moq)
  FormBuilder.sln

frontend/                 אפליקציית Angular 18 (standalone, ללא NgModules)
  src/app/
    core/models            טיפוסים ו-DTOs התואמים את ה-API
    core/services           HttpClient wrapper לתקשורת עם ה-API
    features/form-builder   מסך "יצירת טופס חדש" (בונה שדות + אבני דרך)
    features/forms-list     רשימת טפסים קיימים
    features/form-detail    צפייה בטופס שמור אחד
```

השכבות בצד השרת תלויות בכיוון אחד בלבד: `Api → Infrastructure/Application → Domain`.
ה-Domain לא מכיר EF Core ולא HTTP; ה-Application לא מכיר EF Core (רק ממשקים);
כך אפשר להחליף מסד נתונים או framework בלי לגעת בלוגיקה העסקית.

## הסבר קצר

מבנה הטופס הדינמי (`FormFields`) ומסלול האישורים (`ApprovalSteps`) נשמרים
כטבלאות מנורמלות עם קשר One-to-Many ל-`FormTemplates`, ולא כבלוב HTML/JSON
גולמי — כך אפשר לאכוף מבנה, סדר וחובה כבר ברמת מסד הנתונים, ולבצע ולידציה
ושאילתות אמיתיות על השדות והשלבים. לצורך ה-PoC נעשה שימוש ב-SQLite, אך כל
המודלים מוגדרים ב-EF Core Fluent API בדיוק כפי שהיו מוגדרים מול SQL Server
מלא — מעבר בין השניים הוא שינוי שורה אחת בקובץ `DependencyInjection.cs`.

## שימוש בכלי AI

הפרויקט פותח בעזרת **Claude Code**.
