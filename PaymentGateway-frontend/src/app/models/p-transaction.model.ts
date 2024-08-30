
export interface PTransaction {
    UserId: number;
    TransactionAmount: number;
    PaymentMethod: string;
    TransactionStatus: string;
    TransactionDate: string; // ISO date string
    PaymentMethodId?: string; // Optional
  }
  