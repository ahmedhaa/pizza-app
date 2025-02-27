import { Component, NgZone } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerForm: FormGroup;
  isLoading = false;
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private snackBar: MatSnackBar, private ngZone: NgZone, public dialogRef: MatDialogRef<RegisterComponent>, // Injectez MatDialogRef ici
  ) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.pattern('^(?=.*[A-Za-z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{6,}$')      ]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
      role: ['', [Validators.required, Validators.pattern('^(Admin|User)$')]]
    }, { validators: this.passwordMatchValidator });
  }

  // Validation de la correspondance des mots de passe
  passwordMatchValidator(form: FormGroup): { [key: string]: boolean } | null {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');
    if (password?.value !== confirmPassword?.value) {
      return { 'mismatch': true };
    }
    return null;
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      if (this.registerForm.hasError('mismatch')) {
        this.showErrorToast('Les mots de passe ne correspondent pas.');
      } else if (this.f['password']?.hasError('pattern')) {
        this.showErrorToast('Le mot de passe doit contenir au moins une lettre, un chiffre et un caractère spécial.');
      }
      return; // Ne pas soumettre si des erreurs existent
    }

    const { email, password, confirmPassword, role } = this.registerForm.value;

    this.isLoading = true;
    this.userService.addUser({ email, password, confirmPassword, role }).subscribe(
      (response) => {
        this.isLoading = false;
        console.log('Utilisateur ajouté avec succès:', response);
        this.showSuccessToast('Utilisateur ajouté avec succès');
        this.ngZone.run(() => {
          this.dialogRef.close(true);
        });
      },
      (error) => {
        this.isLoading = false;
        if (error.status === 400 && error.error && error.error.message === "Cet email est déjà utilisé.") {
          this.showErrorToast("Cet email est déjà utilisé. Veuillez en utiliser un autre.");
        } else {
          this.errorMessage = error.error?.message || 'Erreur inconnue';
          this.showErrorToast('Erreur lors de l\'ajout de l\'utilisateur');
        }
      }
    );
  }

  // Méthode pour afficher un message toast
  showSuccessToast(message: string): void {
    this.snackBar.open(message, 'Fermer', {
      duration: 3000,
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: ['toast-success']
    });
  }

  showErrorToast(message: string): void {
    this.snackBar.open(message, 'Fermer', {
      duration: 3000,
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: ['toast-error']
    });
  }

  onCancel(): void {
    this.registerForm.reset();
  }

  get f() {
    return this.registerForm.controls;
  }
}

