export interface PayPayRequest {
    orderId: string;          // Unique identifier for the transaction, e.g., PayPal or Stripe ID
    loginUserId: number;      // User ID of the logged-in user, hardcoded as 3 for now
    expectedAmount: number;   // The amount to be paid, e.g., 30.00
    expectedCurrency: string; // The currency code, e.g., "USD"
  }
  