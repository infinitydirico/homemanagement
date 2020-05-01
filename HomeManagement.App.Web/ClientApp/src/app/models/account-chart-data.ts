export class AccountChartData {
    totalCharges: number;

    outgoingCharges: number

    incomingCharges: number;
}

export class AccountsEvolutionModel {
    public accounts: Array<AccountBalanceModel> = new Array<AccountBalanceModel>();

    public lowestValue: number;
    public highestValue: number;
}

export class AccountBalanceModel {
    accountId: number;
    accountName: string;

    balanceEvolution: Array<MonthBalance>;
}

export class MonthBalance{
    balance: number;
    month: string;
}

export class AccountEvolutionModel{
    outgoingSeries: Array<number> = [];
    incomingSeries: Array<number> = [];
    lowestValue: number;
    highestValue: number;
    
}