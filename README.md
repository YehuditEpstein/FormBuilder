# FormBuilder – בניית טפסים וניהול אבני דרך

אב-טיפוס (PoC) למערכת ליצירת טפסים ארגוניים והגדרת מסלול אישורים דינמי.

**Stack:** Angular 18 (standalone components, Reactive Forms) · ASP.NET Core 8
Web API · EF Core + SQLite.

## מבנה הריפו

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

## הרצת הפרויקט

### שרת (Backend)

דרישה: .NET 8 SDK.

```bash
cd backend
dotnet restore
dotnet run --project src/FormBuilder.Api
```

ה-API עולה על `http://localhost:5000`, עם Swagger UI ב-`http://localhost:5000/swagger`.
מסד הנתונים (`formbuilder.db`, SQLite) נוצר אוטומטית באתחול הראשון
(`Database.EnsureCreated()`), כך שאין צורך בהתקנות נוספות.

הרצת בדיקות היחידה:

```bash
cd backend
dotnet test
```

### לקוח (Frontend)

דרישה: Node.js 20+.

```bash
cd frontend
npm install
npm start
```

האפליקציה עולה על `http://localhost:4200` ומצפה שה-API ירוץ על
`http://localhost:5000/api` (מוגדר ב-`src/environments/environment.ts`).

## מודל הנתונים ונימוקים

| טבלה | תפקיד |
|---|---|
| `FormTemplates` | "מעטפת" הטופס: `Name`, `CreatedAt`, `CreatedBy`. |
| `FormFields` | שדה דינמי בודד בטופס: `Label`, `Type` (Text/Date/Number/…), `OrderIndex`, `IsRequired`, ו-`FormTemplateId` (FK). |
| `ApprovalSteps` | שלב בודד במסלול האישורים: `StepOrder`, `StepName`, `ApproverIdentity`, `ActionType` (Approve/Reject/ApproveOrReject/ViewOnly), ו-`FormTemplateId` (FK). |

**החלטת עיצוב מרכזית:** המבנה הדינמי של הטופס נשמר כרשימת ישויות `FormField`
מנורמלת (טבלה נפרדת + קשר One-to-Many), ולא כבלוב HTML גולמי בעמודת טקסט
בודדת. הסיבה: כך אפשר לשלוט מבנית על סוגי השדות, לאכוף חובה/סדר ברמת ה-DB,
ולהשתמש באותו מבנה מאוחר יותר גם לצורך ולידציה, רינדור דינמי ואפילו האינטגרציה
המתוארת בחלק 3 (שאלה 3) — מחיר ה-HTML הגולמי הוא גמישות רבה יותר אך אפס שליטה
מבנית ואפס אפשרות ולידציה בצד השרת.

באותו אופן, מסלול האישורים הוא טבלה נפרדת (`ApprovalSteps`) ולא עמודת JSON,
כדי לאפשר שאילתות, מיון לפי `StepOrder`, ואכיפת יחסים (Foreign Key + Cascade
Delete) ברמת מסד הנתונים.

לצורך המבחן נעשה שימוש ב-SQLite (קובץ מקומי) כדי לחסוך התקנות, אך כל המודלים
מוגדרים ב-EF Core Fluent API (`Infrastructure/Persistence/Configurations`) בדיוק
כפי שהיו מוגדרים מול SQL Server מלא — מעבר בין השניים הוא שינוי שורה אחת
(`UseSqlite` → `UseSqlServer`) בקובץ `DependencyInjection.cs`.

## נקודות קצה (API)

| Method | Route | תיאור |
|---|---|---|
| `POST` | `/api/form-templates` | שמירת טופס חדש בשלמותו (שדות + מסלול אישורים). |
| `GET` | `/api/form-templates` | שליפת רשימת כל הטפסים (סיכום). |
| `GET` | `/api/form-templates/{id}` | שליפת טופס בודד לפי מזהה, כולל שדות ומסלול. |

## שימוש בכלי AI

> יש להשלים כאן איזה כלי/כלים שימשו בפועל בעבודה על הפרויקט, ולצרף את היסטוריית
> ההתכתבות עמם, בהתאם להנחיות ההגשה.
