import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AppkeyComponent } from './appkey/appkey.component';
import { Liveness2dComponent } from './liveness2d/liveness2d.component';
import { Liveness3dComponent } from './liveness3d/liveness3d.component';
import { IproovComponent } from './iproov/iproov.component';
import { SenddocumentComponent } from './senddocument/senddocument.component';
import { SendDigitalCnhComponent } from './send-digital-cnh/send-digital-cnh.component';
import { FacetecV10Component } from './facetec-v10/facetec-v10.component';

import { ContainerComponent } from './journey/container/container.component';
import { ConsentComponent } from './journey/consent/consent.component';
import { PreparationComponent } from './journey/preparation/preparation.component';
import { CaptureComponent } from './journey/capture/capture.component';
import { ProcessingComponent } from './journey/processing/processing.component';
import { CompletionComponent } from './journey/completion/completion.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';

const routes: Routes = [
  { path: '', component: ContainerComponent }, // Main route acts as the entry
  { path: 'journey/consent', component: ConsentComponent },
  { path: 'journey/preparation', component: PreparationComponent },
  { path: 'journey/capture', component: CaptureComponent },
  { path: 'journey/processing', component: ProcessingComponent },
  { path: 'journey/completion', component: CompletionComponent },
  { path: 'admin', component: AdminDashboardComponent },

  // Legacy components mapping
  { path: 'home', component: HomeComponent },
  { path: 'appkey', component: AppkeyComponent },
  { path: 'liveness-2d', component: Liveness2dComponent },
  { path: 'liveness-3d', component: Liveness3dComponent },
  { path: 'iproov', component: IproovComponent },
  { path: 'facetec-v10', component: FacetecV10Component },
  { path: 'send-document', component: SenddocumentComponent },
  { path: 'send-digital-cnh', component: SendDigitalCnhComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
