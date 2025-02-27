import { Component, OnInit } from '@angular/core';
import { CommandeService } from 'src/app/services/commande.service';
import { CommandeDto, PagedResult } from './Commandesinterfaces/CommandeDto';
import { UpdateStatusDialogComponent } from './update-status-dialog/update-status-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-order-management',
  templateUrl: './order-management.component.html',
  styleUrls: ['./order-management.component.scss']
})
export class OrderManagementComponent implements OnInit {
  commandes: CommandeDto[] = [];  // 'commandes' est un tableau de CommandeDto
  pageNumber: number = 1;
  pageSize: number = 10;

  constructor(private commandeService: CommandeService, private dialog: MatDialog, private snackBar: MatSnackBar) { }
  // Fonction pour afficher le toast
  showToast(message: string): void {
    this.snackBar.open(message, 'Fermer', {
      duration: 3000 // Durée en millisecondes
    });
  }
  ngOnInit() {
    this.getAllCommandes();
  }

  getAllCommandes() {
    this.commandeService.getAllCommandes(this.pageNumber, this.pageSize).subscribe((data: PagedResult<CommandeDto>) => {
      console.log(data);
      if (data && data.items) {
        this.commandes = data.items;  // Assigner 'items' à 'commandes'
      } else {
        console.error('Les données des commandes ne sont pas valides');
      }
    }, error => {
      console.error('Erreur lors de la récupération des commandes:', error);
    });
  }

  viewCommande(id: number) {
    console.log('Afficher la commande avec ID:', id);
    // Tu peux rediriger l'utilisateur vers une page de détails, par exemple :
    // this.router.navigate(['/commande', id]);
  }

  openUpdateStatusDialog(commande: any) {
    const dialogRef = this.dialog.open(UpdateStatusDialogComponent, {
      width: '300px',
      data: { id: commande.id, currentStatus: commande.status }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.updateCommandeStatus(commande.id, result);
      }
    });
  }
  readonly statusMapping: { [key: string]: string } = {
    'EnCours': 'EnCours',
    'EnChemin': 'EnChemin',
    'Livree': 'Livree',
    'A_Traiter': 'A_Traiter'
  };
  updateCommandeStatus(id: number, newStatus: string) {
    // Mapping du statut
    const mappedStatus = this.statusMapping[newStatus] || newStatus;

    const payload = { status: mappedStatus };
    

    console.log(`Mise à jour de la commande ${id} avec le statut :`, payload);

    this.commandeService.updateCommandeStatus(id, payload).subscribe({
      next: response => {
        this.showToast('Statut mis à jour avec succès!');
        console.log('Statut mis à jour avec succès:', response);
        this.getAllCommandes(); // Rafraîchir la liste
      },
      error: error => {
        console.error('Erreur lors de la mise à jour du statut:', error);
      }
    });
  }
}
