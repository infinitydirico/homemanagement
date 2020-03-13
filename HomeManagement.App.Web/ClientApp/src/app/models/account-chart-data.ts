export class AccountChartData {
    totalCharges: number;

    outgoingCharges: number

    incomingCharges: number;
}

export class AccountsEvolutionModel {
    public balances: Array<AccountBalanceModel> = new Array<AccountBalanceModel>();

    public lowestValue: number;
    public highestValue: number;
}

export class AccountBalanceModel {
    accountId: number;
    accountName: string;

    balanceEvolution: Array<number>;
}

export class AccountEvolutionModel{
    outgoingSeries: Array<number> = [];
    incomingSeries: Array<number> = [];
    lowestValue: number;
    highestValue: number;
    
}