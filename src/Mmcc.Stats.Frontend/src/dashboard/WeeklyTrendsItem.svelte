<script lang="ts">
    import type {
        Average,
        Average2 as TpsAverage,
        Result as TpsWeeklyAvgs,
        Result2 as PingsWeeklyAvgs,
    } from "../clients";

    export let thisAvg: Average;
    export let pingsWeeklyAvgs: PingsWeeklyAvgs;
    export let tpsWeeklyAvgs: TpsWeeklyAvgs;

    let differenceString = getDifferenceString(thisAvg.avg, getCorrespondingLastWeek(thisAvg));
    let tpsString = getCorrespondingTpsString(thisAvg);

    function getDifferenceString(thisWeek: number, lastWeek: number): string {
        if (lastWeek === 0) return "+100%";
        const v: number = ((thisWeek - lastWeek) / lastWeek) * 100;
        const vFixed: string = v.toFixed(2);
        if (v >= 0) return `+${vFixed}%`;
        return `${vFixed}%`;
    }

    function getCorrespondingLastWeek(avg: Average): number {
        const res: Average = pingsWeeklyAvgs.lastWeek.find(
            (el) => el.serverName === avg.serverName
        );

        if (res === undefined || res === null) {
            return 0;
        } else {
            return res.avg;
        }
    }

    function getCorrespondingTpsString(avg: Average): string {
        const res: TpsAverage = tpsWeeklyAvgs.averages.find(
            (el) => el.serverName === avg.serverName
        );

        if (res === undefined || res === null) {
            return "N/A";
        } else {
            return res.avg.toFixed(2);
        }
    }
</script>

<div class="lg:mt-4 lg:px-4" id="basic-stats">
    <p class="font-semibold text-sm text-gray-300">{thisAvg.serverName}</p>
    <div class="flex items-center">
        <div class="flex items-center w-64">
            <div class="w-20">
                <p class="text-3xl mb-0">{thisAvg.avg.toFixed(2)}</p>
                <p class="font-light text-xs text-gray-500 -mt-1">
                    avg players
                </p>
            </div>
            <div
                class:bg-green-500={differenceString.startsWith("+")}
                class:bg-red-500={differenceString.startsWith("-")}
                class="rounded text-sm px-2 py-1 ml-3 w-28 md:w-48 text-center"
            >
                <span class="font-bold"
                    >{differenceString}</span
                >
                <span class="font-light hidden md:inline-block">vs last week</span>
            </div>
        </div>
        <div class="border-l border-gray-500 h-8 w-1 mr-3 ml-4" />
        <div>
            <p class="text-3xl mb-0">
                {tpsString}
            </p>
            <p class="font-light text-xs text-gray-500 -mt-1">
                avg tps
            </p>
        </div>
    </div>
</div>
