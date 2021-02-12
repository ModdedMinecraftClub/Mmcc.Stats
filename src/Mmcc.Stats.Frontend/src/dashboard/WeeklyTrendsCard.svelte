<script lang="ts">
    import type {
        Result as TpsWeeklyAvgs,
        Result2 as PingsWeeklyAvgs,
    } from "../clients";
    import WeeklyTrendsItem from "./WeeklyTrendsItem.svelte";

    export let pingsWeeklyAvgs: PingsWeeklyAvgs;
    export let tpsWeeklyAvgs: TpsWeeklyAvgs;
</script>

<div
    id="basic-container"
    class="card flex flex-col lg:h-136 2xl:h-96 lg:flex-wrap gap-y-4 lg:gap-y-0"
>
    {#each pingsWeeklyAvgs.thisWeek as avg}
        <WeeklyTrendsItem thisAvg={avg} pingsWeeklyAvgs={pingsWeeklyAvgs} tpsWeeklyAvgs={tpsWeeklyAvgs} />
    {/each}
</div>
