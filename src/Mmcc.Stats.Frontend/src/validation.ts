export interface IValidationResult {
    errorMsg: string;
}

export function validateDatePair(fromDate: string, toDate: string): IValidationResult {
    if (fromDate === "" || toDate === "" || fromDate === undefined || toDate === undefined) {
        return {
            errorMsg: "Date cannot be empty"
        };
    }

    if (fromDate > toDate) {
        return {
            errorMsg: "From date cannot be bigger than To date"
        };
    }

    return {
        errorMsg: null
    };
}