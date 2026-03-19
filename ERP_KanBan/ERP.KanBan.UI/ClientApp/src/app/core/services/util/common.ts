export class Util {
    static formatDate(date: Date): string {
        const newDate = new Date(date);
        const yy = newDate.getFullYear();
        const mm = newDate.getMonth() + 1; // getMonth() is zero-based
        const dd = newDate.getDate();

        return [yy, (mm > 9 ? '' : '0') + mm, (dd > 9 ? '' : '0') + dd].join('-');
    }
    static formatFullDate(date: Date): string {
        const newDate = new Date(date);
        const yy = newDate.getFullYear();
        const mm = newDate.getMonth() + 1; // getMonth() is zero-based
        const dd = newDate.getDate();
        const hh = newDate.getHours();
        const min = newDate.getMinutes();
        const ss = newDate.getSeconds();

        const formattedDate = [
            yy,
            (mm > 9 ? '' : '0') + mm,
            (dd > 9 ? '' : '0') + dd
        ].join('-');

        const formattedTime = [
            (hh > 9 ? '' : '0') + hh,
            (min > 9 ? '' : '0') + min,
            (ss > 9 ? '' : '0') + ss
        ].join(':');

        return `${formattedDate} ${formattedTime}`;
    }

    static filterChar(str: string) {
        str = str.replace(/\-/g, '');
        return str;
    }
    static firstDayInMonth(): string {
        const date = new Date();
        date.setDate(1);
        return this.formatDate(date);
    }
    static lastDayInMonth(): string {
        const date = new Date();
        const mm = date.getMonth();
        const dd = date.getDate();

        date.setMonth(date.getMonth() + 1);
        date.setDate(1);
        console.log(this.formatDate(date));
        date.setDate(date.getDate() - 1);
        console.log(this.formatDate(date));

        return this.formatDate(date);
    }
    static firstDayInLastMonth(): string {
        const date = new Date();
        date.setMonth(date.getMonth() - 1);
        date.setDate(1);
        return this.formatDate(date);
    }
    static lastDayInLastMonth(): string {

        const date = new Date();
        date.setMonth(date.getMonth());
        date.setDate(1);
        date.setDate(date.getDate() - 1);

        return this.formatDate(date);
    }
    static firstDayInTreeMonth(): string {
        const date = new Date();
        date.setMonth(date.getMonth() - 3);
        date.setDate(1);
        return this.formatDate(date);
    }
    static lastMonth(date: Date) {
        date.setDate(0);
        const d = date;
        let month = '' + (d.getMonth() + 1);
        const year = d.getFullYear();

        if (month.length < 2) { month = '0' + month; }

        return [year, month].join('-');
    }
    static lastMonthYYMM(date: Date) {
        date.setDate(0);
        const d = date;
        let month = '' + (d.getMonth() + 1);
        const year = d.getFullYear();

        if (month.length < 2) { month = '0' + month; }

        return year + month;
    }
    static getMonthStartAndEnd(yyyymm: string) {
        // 解析传入的日期字符串
        const year = parseInt(yyyymm.substring(0, 4), 10);
        const month = parseInt(yyyymm.substring(4, 6), 10) - 1; // 月份从0开始

        // 创建该月份的第一天的日期对象
        const firstDay = new Date(year, month, 1);
        const lastDay = new Date(year, month + 1, 0);

        // 格式化日期为所需的字符串格式
        const formatDate = (date: any, time: any) => {
            const yyyy = date.getFullYear();
            const mm = String(date.getMonth() + 1).padStart(2, '0');
            const dd = String(date.getDate()).padStart(2, '0');
            return `${yyyy}-${mm}-${dd}T${time}`;
        };

        const firstDayString = formatDate(firstDay, "00:00:00");
        const lastDayString = formatDate(lastDay, "23:59:59");

        return {
            firstDay: firstDayString,
            lastDay: lastDayString
        };
    }
    static isExistDate(dateStr: string) { // yyyy/mm/dd
        const dateObj = dateStr.split('-');

        // 列出12個月，每月最大日期限制
        const limitInMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        const theYear = parseInt(dateObj[0], undefined);
        const theMonth = parseInt(dateObj[1], undefined);
        const theDay = parseInt(dateObj[2], undefined);
        const isLeap = new Date(theYear, 1, 29).getDate() === 29; // 是否為閏年?

        if (isLeap) { // 若為閏年，最大日期限制改為 29
            limitInMonth[1] = 29;
        }

        // 比對該日是否超過每個月份最大日期限制
        return theDay <= limitInMonth[theMonth - 1];
    }
    static addDays(days: number): string {
        const date = new Date();
        return this.formatDate(new Date(date.setDate(date.getDate() + days)));
    }
    static addDaysToDate(days: number): Date {
        const date = new Date();
        const _newDate = new Date(date.setDate(date.getDate() + days));
        _newDate.setHours(0, 0, 0);
        return _newDate;
    }
    static yyyymmdd(date: any): string {
        const newDate = new Date(date);
        const yy = newDate.getFullYear();
        const mm = newDate.getMonth() + 1; // getMonth() is zero-based
        const dd = newDate.getDate();

        return [yy, (mm > 9 ? '' : '0') + mm, (dd > 9 ? '' : '0') + dd].join('');
    }
    static yyyymm(date: any): string {
        const newDate = new Date(date);
        const yy = newDate.getFullYear();
        const mm = newDate.getMonth() + 1; // getMonth() is zero-based

        return [yy, (mm > 9 ? '' : '0') + mm].join('');
    }
    static yyyymmHMS(date: Date): string {
        const newDate = new Date(date);
        const yy = newDate.getFullYear();
        const mm = newDate.getMonth() + 1; // getMonth() is zero-based
        const dd = newDate.getDate();
        const hh = newDate.getHours();
        const min = newDate.getMinutes();
        const ss = newDate.getSeconds();

        return [yy, (mm > 9 ? '' : '0') + mm, (dd > 9 ? '' : '0') + dd, (hh > 9 ? '' : '0') + hh, (min > 9 ? '' : '0') + min, (ss > 9 ? '' : '0') + ss].join('');
    }
    static yyyymmddToDate(yyyymmdd: string): any {
        const utcStr = yyyymmdd.substring(0, 4) + '-' + yyyymmdd.substring(4, 6) + '-' + yyyymmdd.substring(6, 8);
        return utcStr + 'T00:00:00';
    }
    static firstTimeInDate(yyyymmdd: string) {
        return yyyymmdd.substring(0, 10) + 'T00:00:00';
    }
    static lastTimeInDate(yyyymmdd: string) {
        return yyyymmdd.substring(0, 10) + 'T23:59:59';
    }
    static today() {
        const date = new Date();
        date.setHours(0, 0, 0, 0);
        return date;
    }

    static firstTimeInLinqDate(yyyymmdd: string) {
        const yyyy = yyyymmdd.substring(0, 4);
        const mm = yyyymmdd.substring(5, 7);
        const dd = yyyymmdd.substring(8, 10);

        return 'DateTime(' + yyyy + ', ' + mm + ', ' + dd + ', 0, 0, 0)';
    }
    static lastTimeInLinqDate(yyyymmdd: string) {
        const yyyy = yyyymmdd.substring(0, 4);
        const mm = yyyymmdd.substring(5, 7);
        const dd = yyyymmdd.substring(8, 10);

        return 'DateTime(' + yyyy + ', ' + mm + ', ' + dd + ', 23, 59, 59)';
    }
    static ddmmyyyy(date: any): string {
        const newDate = new Date(date);
        const yy = newDate.getFullYear();
        const mm = newDate.getMonth() + 1; // getMonth() is zero-based
        const dd = newDate.getDate();

        return [(dd > 9 ? '' : '0') + dd, (mm > 9 ? '' : '0') + mm, yy].join('/');
    }
    static millisecondTimestamp() {
        const date = new Date();
        return date.getTime();
    }
}
