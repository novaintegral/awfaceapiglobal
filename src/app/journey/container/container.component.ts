import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-container',
  templateUrl: './container.component.html',
  styleUrls: ['./container.component.css']
})
export class ContainerComponent implements OnInit {
  constructor(private route: ActivatedRoute, private router: Router, private http: HttpClient) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      const token_tenant = params['token_tenant'];
      const cpf = params['cpf'];
      const nome = params['nome'];
      const nascimento = params['nascimento'];
      const idExternoCliente = params['idExternoCliente'];

      if (token_tenant && cpf) {
        // Init Journey
        this.http.post<any>(`https://localhost:5001/api/journeys/initialize?token_tenant=${token_tenant}&cpf=${cpf}&nome=${nome}&nascimento=${nascimento}&idExternoCliente=${idExternoCliente}`, {})
          .subscribe(res => {
            // Apply whitelabel styles if present
            if (res.tenantTheme) {
               // Logic to apply styles/logo would go here
            }
            // Navigate to consent
            this.router.navigate(['/journey/consent'], { queryParams: { journeyId: res.journeyId }});
          }, err => {
            console.error('Initialization error', err);
          });
      } else {
        // Navigate somewhere or show error
      }
    });
  }
}
