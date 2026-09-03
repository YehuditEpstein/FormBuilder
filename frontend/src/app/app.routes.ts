import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'forms', pathMatch: 'full' },
  {
    path: 'forms',
    loadComponent: () =>
      import('./features/forms-list/forms-list-page.component').then((m) => m.FormsListPageComponent),
  },
  {
    path: 'forms/new',
    loadComponent: () =>
      import('./features/form-builder/form-builder-page.component').then((m) => m.FormBuilderPageComponent),
  },
  {
    path: 'forms/:id',
    loadComponent: () =>
      import('./features/form-detail/form-detail-page.component').then((m) => m.FormDetailPageComponent),
  },
  { path: '**', redirectTo: 'forms' },
];
