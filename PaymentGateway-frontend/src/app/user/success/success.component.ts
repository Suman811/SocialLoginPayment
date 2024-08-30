// import { Component, OnInit } from '@angular/core';
// import { ActivatedRoute, Router } from '@angular/router';

// @Component({
//   selector: 'app-success',
//   templateUrl: './success.component.html',
//   styleUrls: ['./success.component.scss']
// })
// export class SuccessComponent implements OnInit {
//   transactionId: string | null = null;
//   isSuccess: boolean = false;

//   constructor(private route: ActivatedRoute, private router: Router) {}

//   ngOnInit(): void {
//     // Get the transaction ID and success status from the route parameters
//     this.route.queryParams.subscribe(params => {
//       this.transactionId = params['transactionId'];
//       this.isSuccess = params['status'] === 'success';
//     });
//   }

//   goBackToHome(): void {
//     this.router.navigate(['/']); // Navigate back to the home or another component
//   }
// }
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import jsPDF from 'jspdf';
import { AuthServiceService } from 'src/app/services/auth-service.service';

@Component({
  selector: 'app-success',
  templateUrl: './success.component.html',
  styleUrls: ['./success.component.scss']
})
export class SuccessComponent implements OnInit {
  transactionId: string | null = null;
  isSuccess: boolean = false;
  paymentMethod: string | null = null;


  constructor(private route: ActivatedRoute, private router: Router,private authService: AuthServiceService) {}

  ngOnInit(): void {
    // Get the transaction ID and success status from the route parameters
    this.route.queryParams.subscribe(params => {
      this.transactionId = params['transactionId'];
      this.isSuccess = params['status'] === 'success';
      this.paymentMethod = params['method'];

    });
  }

  goBackToHome(): void {
    this.router.navigate(['/']); // Navigate back to the home or another component
  }

  downloadInvoice(): void {
    if (this.transactionId) {
      if (this.paymentMethod === 'stripe') {
        this.authService.getTransactionDetails(this.transactionId).subscribe(transaction => {
          this.generatePdf(transaction);
        }, error => {
          console.error('Error fetching invoice URL:', error);
        });
      } else {
        this.authService.getInvoice(this.transactionId).subscribe(transaction => {
          this.generatePdf(transaction);
        }, error => {
          console.error('Error fetching transaction:', error);
        });
      }
    }
  }


  generatePdf(transaction: any): void {
    const doc = new jsPDF();

    // Company branding
    doc.setFontSize(16);
    doc.setFont('helvetica', 'bold');
    doc.text('Vivek Enterprises', 10, 10);

    // Title
    doc.setFontSize(22);
    doc.setFont('helvetica', 'bold');
    doc.text('Invoice', 10, 20);

    // Line separator
    doc.setLineWidth(0.5);
    doc.line(10, 25, 200, 25);

    // Reset font to regular
    doc.setFontSize(12);
    doc.setFont('helvetica', 'normal');

    // Invoice details
    doc.text(`Transaction ID: ${transaction.transactionId}`, 10, 35);
    doc.text(`Amount: $${transaction.amount.toFixed(2)}`, 10, 45);
    doc.text(`Currency: ${transaction.currency}`, 10, 55);
    doc.text(`Status: ${transaction.status}`, 10, 65);
    doc.text(`Time of Transaction: ${this.formatDate(transaction.timeOfTransaction)}`, 10, 75);

    // Footer
    doc.setFontSize(10);
    doc.text('Thank you for your business!', 10, 90);
    doc.text('For any inquiries, please contact us at support@vivekent.com', 10, 95);

    // Save the PDF
    doc.save('invoice.pdf');
  }

  // Helper method to format date
  private formatDate(dateString: string): string {
    if (!dateString || dateString === '0001-01-01 00:00:00') {
      return 'N/A';
    }
    const date = new Date(dateString);
    return `${date.getFullYear()}-${('0' + (date.getMonth() + 1)).slice(-2)}-${('0' + date.getDate()).slice(-2)} ${('0' + date.getHours()).slice(-2)}:${('0' + date.getMinutes()).slice(-2)}:${('0' + date.getSeconds()).slice(-2)}`;
  }
}
