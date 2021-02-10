<script lang="ts">
    import type {
        Average,
        Average2 as TpsAverage,
        Result as TpsWeeklyAvgs,
        Result2 as PingsWeeklyAvgs,
    } from "../clients";
    import WeeklyTrendsItem from "./WeeklyTrendsItem.svelte";

    export let pingsWeeklyAvgs: PingsWeeklyAvgs;
    export let tpsWeeklyAvgs: TpsWeeklyAvgs;

    function getDifferenceString(thisWeek: number, lastWeek: number): string {
        if (lastWeek === 0) return "+100%";
        const v: number = ((thisWeek - lastWeek) / lastWeek) * 100;
        if (v >= 0) return `+${v}%`;
        return `-${v}%`;
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

<div
    id="basic-container"
    class="card flex flex-col lg:h-96 lg:flex-wrap pt-56 gap-y-4 lg:gap-y-0"
>
    {#each pingsWeeklyAvgs.thisWeek as avg}
        <WeeklyTrendsItem thisAvg={avg} pingsWeeklyAvgs={pingsWeeklyAvgs} tpsWeeklyAvgs={tpsWeeklyAvgs} />
    {/each}
</div>
