import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiServiceService } from '../service/api-service.service';
import { Satcom } from '../models/satcom.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-satcom-form',
  templateUrl: './satcom-form.component.html',
  styleUrls: ['./satcom-form.component.scss']
})
export class SatcomFormComponent implements OnInit {
  satcomForm: FormGroup;
  isEditMode = false;
  satcomId: number | null = null;
  loading = false;

  categories = ['Online', 'Virtual', 'Hybrid', 'Physical'];
  types = ['EGM', 'AGM', 'AGM & EGM', 'UHM'];

  constructor(
    private fb: FormBuilder,
    private apiService: ApiServiceService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.satcomForm = this.fb.group({
      company: ['', [Validators.required]],
      category: ['', [Validators.required]],
      type: ['', [Validators.required]],
      date: ['', [Validators.required]],
      time: ['', [Validators.required]],
      db_port: ['', [Validators.required]],
      ui_port: ['', [Validators.required]],
      url: ['', [Validators.required]],
      ip: ['', [Validators.required, Validators.pattern(/^(?:(?:25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)\.){3}(?:25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)$/)]],
      status: [true]
    });
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.satcomId = +id;
      this.loadSatcomData(this.satcomId);
    }
  }

  loadSatcomData(id: number) {
    this.loading = true;
    this.apiService.getOneData(id).subscribe(
      (res) => {
        this.loading = false;
        if (res.isSuccess && res.payload) {
          // Handle both array and single object responses
          const data = Array.isArray(res.payload) ? res.payload[0] : res.payload;
          if (data) {
            this.satcomForm.patchValue({
              company: data.company,
              category: data.category,
              type: data.type,
              date: data.date,
              time: data.time,
              db_port: data.db_port,
              ui_port: data.ui_port,
              url: data.url,
              ip: data.ip,
              status: data.status
            });
          }
        }
      },
      (error) => {
        this.loading = false;
        Swal.fire('Error', error?.error?.serviceMessage || 'Failed to load company data', 'error');
        this.router.navigate(['/home']);
      }
    );
  }

  onSubmit() {
    if (this.satcomForm.valid) {
      this.loading = true;
      const formData: Satcom = this.satcomForm.value;

      if (this.isEditMode && this.satcomId) {
        // Update existing data
        this.apiService.updateSatcom(this.satcomId, formData).subscribe(
          (res) => {
            this.loading = false;
            if (res.isSuccess) {
              Swal.fire('Success', res.serviceMessage, 'success');
              this.router.navigate(['/home']);
            }
          },
          (error) => {
            this.loading = false;
            Swal.fire('Error', error?.error?.serviceMessage || 'Failed to update company data', 'error');
          }
        );
      } else {
        // Create new data
        this.apiService.createSatcom(formData).subscribe(
          (res) => {
            this.loading = false;
            if (res.isSuccess) {
              Swal.fire('Success', res.serviceMessage, 'success');
              this.router.navigate(['/home']);
            }
          },
          (error) => {
            this.loading = false;
            Swal.fire('Error', error?.error?.serviceMessage || 'Failed to create company data', 'error');
          }
        );
      }
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.satcomForm.controls).forEach(key => {
        this.satcomForm.get(key)?.markAsTouched();
      });
      Swal.fire('Validation Error', 'Please fill all required fields correctly', 'warning');
    }
  }

  cancel() {
    this.router.navigate(['/home']);
  }

  get f() {
    return this.satcomForm.controls;
  }
}
