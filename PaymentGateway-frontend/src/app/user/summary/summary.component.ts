// import { Component, OnInit } from '@angular/core';
// import { ActivatedRoute, Router } from '@angular/router';
// import { ICreateOrderRequest, IPayPalConfig } from 'ngx-paypal';
// import { AuthServiceService } from 'src/app/services/auth-service.service';
// import { loadStripe } from '@stripe/stripe-js';
// import { PaymentRequest } from 'src/app/models/PaymentRequest';
// import { PayPayRequest } from 'src/app/models/PayPalRequest';

// @Component({
//   selector: 'app-summary',
//   templateUrl: './summary.component.html',
//   styleUrls: ['./summary.component.scss']
// })
// export class SummaryComponent implements OnInit {
//   stripePromise = loadStripe('pk_test_51HxRkiCumzEESdU2Z1FzfCVAJyiVHyHifo0GeCMAyzHPFme6v6ahYeYbQPpD9BvXbAacO2yFQ8ETlKjo4pkHSHSh00qKzqUVK9');
//   selectedMethod: string = '';
//   paymentOptionsVisible: boolean = false;
//   plan = {
//     name: '',
//     price: '',
//     users: '',
//     storage: '',
//     support: ''
//   };
//   public payPalConfig?: IPayPalConfig;
//   showSuccess: boolean = false;
//   showCancel: boolean = false;
//   showError: boolean = false;
//   handler: any = null;
//   email: string = 'user@example.com'; 

//   constructor(
//     private route: ActivatedRoute,
//     private router: Router,
//     private paymentService: AuthServiceService 
//   ) {}

//   ngOnInit(): void {
//     this.route.queryParams.subscribe(params => {
//       const selectedPlan = params['plan'];
//       this.setPlanDetails(selectedPlan);
//     });

//     this.initConfig();
//     this.loadStripe(); 
//   }

//   showPaymentOptions(): void {
//     this.paymentOptionsVisible = true;
//   }

//   goBack(): void {
//     this.paymentOptionsVisible = false;
//   }

//   confirmSubscription(): void {
//     this.showPaymentOptions();
//     console.log('Subscription confirmed for:', this.plan);

//     // Initiate Stripe payment
//     if (this.selectedMethod === 'Stripe') {
//       this.payWithStripe();
//     }
//   }

//   setPlanDetails(planType: string): void {
//     switch (planType) {
//       case 'basic':
//         this.plan = {
//           name: 'Basic Plan',
//           price: '$10/month',
//           users: '1 User',
//           storage: '10 GB Storage',
//           support: 'Basic Support'
//         };
//         break;
//       case 'standard':
//         this.plan = {
//           name: 'Standard Plan',
//           price: '$30/month',
//           users: '5 Users',
//           storage: '50 GB Storage',
//           support: 'Priority Support'
//         };
//         break;
//       case 'premium':
//         this.plan = {
//           name: 'Premium Plan',
//           price: '$60/month',
//           users: 'Unlimited Users',
//           storage: '200 GB Storage',
//           support: '24/7 Support'
//         };
//         break;
//       default:
//         this.plan = {
//           name: 'Unknown Plan',
//           price: 'N/A',
//           users: 'N/A',
//           storage: 'N/A',
//           support: 'N/A'
//         };
//         break;
//     }
//   }

//   private initConfig(): void {
//     this.payPalConfig = {
//       currency: 'USD',
//       clientId: 'AfA6zF4OppdK8GwL4GBU4l8K_KrCH3ZMzQBX5oozJ6eCJ1D_SRq-j1nx-xmYOfdkDsoSTj01AyOClIOb',
//       createOrderOnClient: (data) => <ICreateOrderRequest>{
//         intent: 'CAPTURE',
//         purchase_units: [{
//           amount: {
//             currency_code: 'USD',
//             value: this.getPlanAmount().toFixed(2),
//             breakdown: {
//               item_total: {
//                 currency_code: 'USD',
//                 value: this.getPlanAmount().toFixed(2)
//               }
//             }
//           },
//           items: [{ 
//             name: this.plan.name,
//             quantity: '1',
//             category: 'DIGITAL_GOODS',
//             unit_amount: {
//               currency_code: 'USD',
//               value: this.getPlanAmount().toFixed(2),
//             },
//           }]
//         }]
//       },
//       advanced: {
//         commit: 'true'
//       },
//       style: {
//         label: 'paypal',
//         layout: 'vertical'
//       },
//       onApprove: (data, actions) => {
//         console.log('onApprove - transaction was approved, but not authorized', data, actions);
//         actions.order.get().then((details: any) => {
//           console.log('onApprove - you can get full order details inside onApprove: ', details);
//         }); 
//       },
//       onClientAuthorization: (data) => {
//         console.log('onClientAuthorization - transaction completed successfully', data);
//         this.saveTransactionData('PayPal', 'success', data);
//         this.showSuccess = true;
//         this.router.navigate(['user/success'], { queryParams: { transactionId: data.id, status: 'success' } });
//       },
//       onCancel: (data, actions) => {
//         console.log('OnCancel', data, actions);
//         this.showCancel = true;
//         this.saveTransactionData('PayPal', 'cancelled', data);
//         this.router.navigate(['user/success'], { queryParams: { status: 'cancelled' } });
//       },
//       onError: err => {
//         console.log('OnError', err);
//         this.showError = true;
//         this.saveTransactionData('PayPal', 'failure', err);
//         this.router.navigate(['user/success'], { queryParams: { status: 'failure' } });
//       },
//       onClick: (data, actions) => {
//         console.log('onClick', data, actions);
//         this.resetStatus();
//       }
//     };
//   }

//   private loadStripe(): void {
//     if (!window.document.getElementById('stripe-script')) {
//       const s = window.document.createElement("script");
//       s.id = "stripe-script";
//       s.type = "text/javascript";
//       s.src = "https://checkout.stripe.com/checkout.js";
//       s.onload = () => {
//         this.handler = (<any>window).StripeCheckout.configure({
//           key: 'pk_test_51HxRkiCumzEESdU2Z1FzfCVAJyiVHyHifo0GeCMAyzHPFme6v6ahYeYbQPpD9BvXbAacO2yFQ8ETlKjo4pkHSHSh00qKzqUVK9',
//           locale: 'auto',
//           token: (token: any) => {
//             console.log(token);
//             alert('Payment Success!!');
//             this.saveTransactionData('Stripe', 'success', { id: token.id }, this.email, token.id);
//             this.router.navigate(['user/success'], { queryParams: { transactionId: token.id, status: 'success' , method:'stripe'} });
//           }
//         });
//       }
//       window.document.body.appendChild(s);
//     }
//   }

//   public payWithStripe(): void {
//     const amount = this.getPlanAmount(); 

//     if (this.handler) {
//       this.handler.open({
//         name: 'Professional Stripe',
//         description: 'Subscription Payment',
//         amount: amount * 100 // Convert dollars to cents
//       });
//     } else {
//       console.error('Stripe handler is not initialized.');
//     }
//   }
  
//   private getPlanAmount(): number {
//     return parseFloat(this.plan.price.replace('$', '').replace('/month', ''));
//   }

//   private saveTransactionData(paymentMethod: string, transactionStatus: string, response: any, email?: string, transactionId?: string): void {
//     if (paymentMethod === 'PayPal') {
//       this.handlePayPalTransaction(response);
//     } else if (paymentMethod === 'Stripe') {
//       this.handleStripeTransaction(email || '', transactionId || '');
//     }
//   }

//   private handlePayPalTransaction(response: any): void {
//     const transactionData: PayPayRequest = {
//       orderId: response.id,
//       loginUserId: 3,
//       expectedAmount: this.getPlanAmount(),
//       expectedCurrency: 'USD',
//     };

//     this.paymentService.savePayPalTransaction(transactionData).subscribe(
//       response => {
//         console.log('PayPal transaction data saved successfully:', response);
//       },
//       error => {
//         console.error('Error saving PayPal transaction data:', error);
//       }
//     );
//   }

//   private handleStripeTransaction(email: string, transactionId: string): void {
//     const transactionData: PaymentRequest = {
//       userId: 3,  // Assuming the user ID is 3
//       email: email,  // Use the provided email
//       sourceToken: transactionId,  // Assuming transactionId is the source token
//       amount: this.getPlanAmount()*100,  // Use the plan's amount
//     };

//     this.paymentService.saveStripeTransaction(transactionData).subscribe(
//       response => {
//         console.log('Stripe transaction data saved successfully:', response);
//       },
//       error => {
//         console.error('Error saving Stripe transaction data:', error);
//       }
//     );
//   }
  

//   private resetStatus(): void {
//     this.showSuccess = false;
//     this.showCancel = false;
//     this.showError = false;
//   }
// }


// ////////////////////////////////////////////////
// //final testing
// ////////////



import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ICreateOrderRequest, IPayPalConfig } from 'ngx-paypal';
import { AuthServiceService } from 'src/app/services/auth-service.service';
import { loadStripe } from '@stripe/stripe-js';
import { PaymentRequest } from 'src/app/models/PaymentRequest';
import { PayPayRequest } from 'src/app/models/PayPalRequest';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.scss']
})
export class SummaryComponent implements OnInit {
  stripePromise = loadStripe('pk_test_51HxRkiCumzEESdU2Z1FzfCVAJyiVHyHifo0GeCMAyzHPFme6v6ahYeYbQPpD9BvXbAacO2yFQ8ETlKjo4pkHSHSh00qKzqUVK9');
  selectedMethod: string = '';
  paymentOptionsVisible: boolean = false;
  plan = {
    name: '',
    price: '',
    users: '',
    storage: '',
    support: ''
  };
  public payPalConfig?: IPayPalConfig;
  showSuccess: boolean = false;
  showCancel: boolean = false;
  showError: boolean = false;
  email: string = 'user@example.com';
  handler: any = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private paymentService: AuthServiceService 
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const selectedPlan = params['plan'];
      this.setPlanDetails(selectedPlan);
    });

    this.initConfig();
    this.loadRazorpay();
    this.loadStripe();
  }

  showPaymentOptions(): void {
    this.paymentOptionsVisible = true;
  }

  goBack(): void {
    this.paymentOptionsVisible = false;
  }

  confirmSubscription(): void {
    this.showPaymentOptions();
    console.log('Subscription confirmed for:', this.plan);

    if (this.selectedMethod === 'stripe') {
      this.payWithStripe();
    } else if (this.selectedMethod === 'paypal') {
      this.payWithPayPal();
    } else if (this.selectedMethod === 'razorpay') {
      this.payWithRazorpay();
    }
  }

  setPlanDetails(planType: string): void {
    switch (planType) {
      case 'basic':
        this.plan = {
          name: 'Basic Plan',
          price: '$10/month',
          users: '1 User',
          storage: '10 GB Storage',
          support: 'Basic Support'
        };
        break;
      case 'standard':
        this.plan = {
          name: 'Standard Plan',
          price: '$30/month',
          users: '5 Users',
          storage: '50 GB Storage',
          support: 'Priority Support'
        };
        break;
      case 'premium':
        this.plan = {
          name: 'Premium Plan',
          price: '$60/month',
          users: 'Unlimited Users',
          storage: '200 GB Storage',
          support: '24/7 Support'
        };
        break;
      default:
        this.plan = {
          name: 'Unknown Plan',
          price: 'N/A',
          users: 'N/A',
          storage: 'N/A',
          support: 'N/A'
        };
        break;
    }
  }

  private initConfig(): void {
    this.payPalConfig = {
      currency: 'USD',
      clientId: 'AfA6zF4OppdK8GwL4GBU4l8K_KrCH3ZMzQBX5oozJ6eCJ1D_SRq-j1nx-xmYOfdkDsoSTj01AyOClIOb',
      createOrderOnClient: (data) => <ICreateOrderRequest>{
        intent: 'CAPTURE',
        purchase_units: [{
          amount: {
            currency_code: 'USD',
            value: this.getPlanAmount().toFixed(2),
            breakdown: {
              item_total: {
                currency_code: 'USD',
                value: this.getPlanAmount().toFixed(2)
              }
            }
          },
          items: [{ 
            name: this.plan.name,
            quantity: '1',
            category: 'DIGITAL_GOODS',
            unit_amount: {
              currency_code: 'USD',
              value: this.getPlanAmount().toFixed(2),
            },
          }]
        }]
      },
      advanced: {
        commit: 'true'
      },
      style: {
        label: 'paypal',
        layout: 'vertical'
      },
      onApprove: (data, actions) => {
        console.log('onApprove - transaction was approved, but not authorized', data, actions);
        actions.order.get().then((details: any) => {
          console.log('onApprove - you can get full order details inside onApprove: ', details);
        }); 
      },
      onClientAuthorization: (data) => {
        console.log('onClientAuthorization - transaction completed successfully', data);
        this.saveTransactionData('PayPal', 'success', data);
        this.showSuccess = true;
        this.router.navigate(['user/success'], { queryParams: { transactionId: data.id, status: 'success' } });
      },
      onCancel: (data, actions) => {
        console.log('OnCancel', data, actions);
        this.showCancel = true;
        this.saveTransactionData('PayPal', 'cancelled', data);
        this.router.navigate(['user/success'], { queryParams: { status: 'cancelled' } });
      },
      onError: err => {
        console.log('OnError', err);
        this.showError = true;
        this.saveTransactionData('PayPal', 'failure', err);
        this.router.navigate(['user/success'], { queryParams: { status: 'failure' } });
      },
      onClick: (data, actions) => {
        console.log('onClick', data, actions);
        this.resetStatus();
      }
    };
  }

  private loadStripe(): void {
    if (!window.document.getElementById('stripe-script')) {
      const s = window.document.createElement("script");
      s.id = "stripe-script";
      s.type = "text/javascript";
      s.src = "https://checkout.stripe.com/checkout.js";
      s.onload = () => {
        this.handler = (<any>window).StripeCheckout.configure({
          key: 'pk_test_51HxRkiCumzEESdU2Z1FzfCVAJyiVHyHifo0GeCMAyzHPFme6v6ahYeYbQPpD9BvXbAacO2yFQ8ETlKjo4pkHSHSh00qKzqUVK9',
          locale: 'auto',
          token: (token: any) => {
            console.log(token);
            this.saveTransactionData('Stripe', 'success', { id: token.id }, this.email, token.id);
            this.showSuccess = true;
            this.router.navigate(['user/success'], { queryParams: { transactionId: token.id, status: 'success' , method:'stripe'} });
          }
        });
      }
      window.document.body.appendChild(s);
    }
  }

  private loadRazorpay(): void {
    if (!(window as any).Razorpay) {
      const script = window.document.createElement('script');
      script.id = 'razorpay-script';
      script.src = 'https://checkout.razorpay.com/v1/checkout.js';
      script.onload = () => {
        console.log('Razorpay script loaded');
      };
      window.document.body.appendChild(script);
    }
  }

  public payWithRazorpay(): void {
    if (!(window as any).Razorpay) {
      console.error('Razorpay SDK not loaded.');
      return;
    }

    const amount = this.getPlanAmount() * 100; // Convert dollars to paise

    const options: any = {
      key: 'rzp_test_EpGj4VZNNTxqi2', // Replace with your Razorpay key
      amount: amount.toString(), // Amount in paise
      currency: 'USD',
      name: 'Subscription Payment',
      description: 'Payment for subscription plan',
      handler: (response: any) => {
        console.log('Payment successful', response);
        this.saveTransactionData('Razorpay', 'success', response);
        this.showSuccess = true;
        this.router.navigate(['user/success'], { queryParams: { transactionId: response.razorpay_payment_id, status: 'success' } });
      },
      prefill: {
        email: this.email,
      },
      theme: {
        color: '#3399cc'
      }
    };

    const paymentObject = new (window as any).Razorpay(options);
    paymentObject.open();
  }

  public payWithStripe(): void {
    const amount = this.getPlanAmount() * 100; // Convert dollars to cents

    if (this.handler) {
      this.handler.open({
        name: 'Subscription Payment',
        description: this.plan.name,
        amount: amount
      });
    } else {
      console.error('Stripe handler is not initialized.');
    }
  }

  public payWithPayPal(): void {
    // This will trigger PayPal payment via PayPal button component
  }

  private getPlanAmount(): number {
    return parseFloat(this.plan.price.replace('$', '').replace('/month', ''));
  }

  private saveTransactionData(paymentMethod: string, transactionStatus: string, response: any, email?: string, transactionId?: string): void {
    if (paymentMethod === 'Razorpay') {
        this.handleRazorpayTransaction(response);
    } else if (paymentMethod === 'Stripe') {
        this.handleStripeTransaction(email || '', transactionId || '');
    } else if (paymentMethod === 'PayPal') {
        this.handlePayPalTransaction(response);
    }
    // Optionally: Add logic to save transaction data to your backend if needed
}

private handleRazorpayTransaction(response: any): void {
    const transactionData: PaymentRequest = {
        userId: 3,  // Assuming the user ID is 3
        email: this.email,  // Use the provided email
        sourceToken: response.razorpay_payment_id,  // Payment ID from Razorpay
        amount: this.getPlanAmount() * 100,  // Use the plan's amount
    };

    // this.paymentService.saveRazorpayTransaction(transactionData).subscribe(
    //     response => {
    //         console.log('Razorpay transaction data saved successfully:', response);
    //     },
    //     error => {
    //         console.error('Error saving Razorpay transaction data:', error);
    //     }
    // );
}

private handlePayPalTransaction(response: any): void {
    const transactionData: PayPayRequest = {
        orderId: response.id,
        loginUserId: 3,
        expectedAmount: this.getPlanAmount(),
        expectedCurrency: 'USD',
    };

    this.paymentService.savePayPalTransaction(transactionData).subscribe(
        response => {
            console.log('PayPal transaction data saved successfully:', response);
        },
        error => {
            console.error('Error saving PayPal transaction data:', error);
        }
    );
}

private handleStripeTransaction(email: string, transactionId: string): void {
    const transactionData: PaymentRequest = {
        userId: 3,  // Assuming the user ID is 3
        email: email,  // Use the provided email
        sourceToken: transactionId,  // Assuming transactionId is the source token
        amount: this.getPlanAmount() * 100,  // Use the plan's amount
    };

    this.paymentService.saveStripeTransaction(transactionData).subscribe(
        response => {
            console.log('Stripe transaction data saved successfully:', response);
        },
        error => {
            console.error('Error saving Stripe transaction data:', error);
        }
    );
}

private resetStatus(): void {
    this.showSuccess = false;
    this.showCancel = false;
    this.showError = false;
}

}


////////////////////////////////////////////////
//final testing
////////////