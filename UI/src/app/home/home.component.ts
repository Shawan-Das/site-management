import { Component, OnInit } from '@angular/core';
import { AuthService } from '../service/auth.service';
import { Router } from '@angular/router';
import { ApiServiceService } from '../service/api-service.service';
import { Satcom } from '../models/satcom.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  companyList: Satcom[] = [];
  loading = false;
  showFormModal = false;
  isEditMode = false;
  satcomId: number | null = null;
  formLoading = false;
  satcomForm: FormGroup;

  categories = ['Online', 'Virtual', 'Hybrid', 'Physical'];
  types = ['EGM', 'AGM', 'AGM & EGM', 'UHM'];

  constructor(
    private authService: AuthService,
    private router: Router,
    private apiService: ApiServiceService,
    private fb: FormBuilder
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
      ip: ['', [Validators.required, Validators.pattern(/^(\d{1,3}\.){3}\d{1,3}$/)]],
      status: [true]
    });
  }

  ngOnInit() {
    this.getAllData();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  getAllData() {
    this.loading = true;
    this.apiService.getCompanyList().subscribe(
      (res) => {
        this.loading = false;
        if (res.isSuccess && res.payload) {
          this.companyList = res.payload;
        }
      },
      (error) => {
        this.loading = false;
        Swal.fire('Error', error?.error?.serviceMessage || 'Failed to load company data', 'error');
      }
    );
  }

  openCreateModal() {
    this.isEditMode = false;
    this.satcomId = null;
    this.satcomForm.reset({
      status: true
    });
    this.showFormModal = true;
    document.body.style.overflow = 'hidden';
  }

  openEditModal(id: number) {
    this.isEditMode = true;
    this.satcomId = id;
    this.formLoading = true;
    this.showFormModal = true;
    document.body.style.overflow = 'hidden';
    
    this.apiService.getOneData(id).subscribe(
      (res) => {
        this.formLoading = false;
        if (res.isSuccess && res.payload) {
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
        this.formLoading = false;
        Swal.fire('Error', error?.error?.serviceMessage || 'Failed to load company data', 'error');
        this.closeFormModal();
      }
    );
  }

  closeFormModal() {
    this.showFormModal = false;
    document.body.style.overflow = 'auto';
    this.satcomForm.reset({
      status: true
    });
  }

  onSubmitForm() {
    if (this.satcomForm.valid) {
      this.formLoading = true;
      const formData: Satcom = this.satcomForm.value;

      if (this.isEditMode && this.satcomId) {
        this.apiService.updateSatcom(this.satcomId, formData).subscribe(
          (res) => {
            this.formLoading = false;
            if (res.isSuccess) {
              Swal.fire({
                icon: 'success',
                title: 'Success!',
                text: res.serviceMessage,
                timer: 2000,
                showConfirmButton: false
              });
              this.closeFormModal();
              this.getAllData();
            }
          },
          (error) => {
            this.formLoading = false;
            Swal.fire('Error', error?.error?.serviceMessage || 'Failed to update company data', 'error');
          }
        );
      } else {
        this.apiService.createSatcom(formData).subscribe(
          (res) => {
            this.formLoading = false;
            if (res.isSuccess) {
              Swal.fire({
                icon: 'success',
                title: 'Success!',
                text: res.serviceMessage,
                timer: 2000,
                showConfirmButton: false
              });
              this.closeFormModal();
              this.getAllData();
            }
          },
          (error) => {
            this.formLoading = false;
            Swal.fire('Error', error?.error?.serviceMessage || 'Failed to create company data', 'error');
          }
        );
      }
    } else {
      Object.keys(this.satcomForm.controls).forEach(key => {
        this.satcomForm.get(key)?.markAsTouched();
      });
      Swal.fire('Validation Error', 'Please fill all required fields correctly', 'warning');
    }
  }

  viewUrl(url: string, status: boolean) {
    if (status && url) {
      window.open(url, '_blank');
    }
  }

  deleteCompany(id: number, companyName: string) {
    Swal.fire({
      title: 'Are you sure?',
      text: `Do you want to delete ${companyName}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        this.apiService.deleteSatcom(id).subscribe(
          (res) => {
            if (res.isSuccess) {
              Swal.fire('Deleted!', res.serviceMessage, 'success');
              this.getAllData();
            }
          },
          (error) => {
            Swal.fire('Error', error?.error?.serviceMessage || 'Failed to delete company data', 'error');
          }
        );
      }
    });
  }

  get f() {
    return this.satcomForm.controls;
  }
}
