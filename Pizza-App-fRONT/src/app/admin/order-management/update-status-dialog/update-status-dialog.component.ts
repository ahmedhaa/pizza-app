import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-update-status-dialog',
  templateUrl: './update-status-dialog.component.html',
  styleUrls: ['./update-status-dialog.component.scss']
})
export class UpdateStatusDialogComponent {
  statusForm: FormGroup;
  statusOptions = ['EnCours', 'Livree', 'EnChemin','A_Traiter'];

  constructor(
    public dialogRef: MatDialogRef<UpdateStatusDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { id: number, currentStatus: string },
    private fb: FormBuilder
  ) {
    this.statusForm = this.fb.group({
      status: [data.currentStatus, Validators.required]
    });
  }

  updateStatus() {
    if (this.statusForm.valid) {
      this.dialogRef.close(this.statusForm.value.status);
    }
  }

  closeDialog() {
    this.dialogRef.close();
  }
}
