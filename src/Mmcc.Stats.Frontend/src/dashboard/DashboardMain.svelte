<script lang="ts">
    import apiConfig from "../apiConfig";
    import {
        PingsClient,
        Result as TpsWeeklyAvgs,
        Result2 as PingsWeeklyAvgs,
        TpsClient,
    } from "../clients";
    import Loading from "../Loading.svelte";
    import PlayerbaseChartCard from "./PlayerbaseChartCard.svelte";
    import WeeklyTrendsCard from "./WeeklyTrendsCard.svelte";
    import { fade } from "svelte/transition";
    import TpsChartCard from "./TpsChartCard.svelte";

    interface IBasicInfo {
        tpsAvg: TpsWeeklyAvgs;
        pingsAvgs: PingsWeeklyAvgs;
    }

    const tpsClient: TpsClient = new TpsClient(apiConfig);
    const pingsClient: PingsClient = new PingsClient(apiConfig);

    async function loadBasic(): Promise<IBasicInfo> {
        let tpsRes: TpsWeeklyAvgs = (await tpsClient.getWeeklyAvgs());
        let pingsRes: PingsWeeklyAvgs = await pingsClient.getWeeklyAvgs();

        return {
            tpsAvg: tpsRes,
            pingsAvgs: pingsRes,
        };
    }
</script>

<div class="bg-gray-900 text-gray-100 py-16 px-6 mb-12 lg:px-10">
    {#await loadBasic()}
        <div in:fade={{delay: 400, duration: 300}} out:fade={{duration: 300}} wmode="transparent">
            <Loading />
        </div>
    {:then res}
        <div in:fade={{delay: 400, duration: 300}} out:fade={{duration: 300}} wmode="transparent">
            <div id="weekly-trends">
                <h2 class="my-2 text-lg ml-1 text-gray-300 uppercase tracking-wide">
                    Weekly trends
                </h2>
                <WeeklyTrendsCard pingsWeeklyAvgs={res.pingsAvgs} tpsWeeklyAvgs={res.tpsAvg} />
            </div>
            <div id="charts">
                <h2 class="mt-12 mb-2 text-lg ml-1 text-gray-300 uppercase tracking-wide">
                    Charts
                </h2>
                <div class="flex flex-col gap-4 lg:grid lg:grid-cols-2 lg:gap-x-8">
                    <PlayerbaseChartCard pingsClient={pingsClient} />
                    <TpsChartCard tpsClient={tpsClient} />
                </div>
            </div>
        </div>
    {/await}
</div>
