export interface IDatePeriod {
    fromDate: Date,
    toDate: Date,
}

export interface IDatePeriodStrings {
    fromDate: string,
    toDate: string,
}

export function toHtmlInputFormat(date: Date): string {
    return date.toISOString().slice(0,10);
}

export function getSevenDaysAgo(): Date {
    return new Date(Date.now() - 7 * 24 * 60 * 60 * 1000);
}

export function getDefaultInputDates(): IDatePeriodStrings {
    const todayString: string = toHtmlInputFormat(new Date());
    const lastWeekString: string = toHtmlInputFormat(getSevenDaysAgo());

    return {
        fromDate: lastWeekString,
        toDate: todayString
    };
}
