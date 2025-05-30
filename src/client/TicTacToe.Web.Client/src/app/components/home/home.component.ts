import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";

import { RegisterFlowComponent } from "./shared/components/register-flow/register-flow.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RegisterFlowComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent { }