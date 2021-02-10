<script lang="ts">
    import apiConfig from "../apiConfig";
    import {
        PingsClient,
        Result as TpsWeeklyAvgs,
        Result2 as PingsWeeklyAvgs,
        TpsClient,
    } from "../clients";
    import Loading from "../Loading.svelte";
    import PlayerbaseCard from "./PlayerbaseCard.svelte";
    import TpsCard from "./TpsCard.svelte";

    interface IBasicInfo {
        tpsAvg: TpsWeeklyAvgs;
        pingsAvgs: PingsWeeklyAvgs;
    }

    const tpsClient: TpsClient = new TpsClient(apiConfig);
    const pingsClient: PingsClient = new PingsClient(apiConfig);

    async function loadBasic(): Promise<IBasicInfo> {
        let tpsRes: TpsWeeklyAvgs = (await tpsClient.getWeeklyAvgs());
        let pingsRes: PingsWeeklyAvgs = await pingsClient.getWeeklyAvgs();

        console.log(pingsRes);

        return {
            tpsAvg: tpsRes,
            pingsAvgs: pingsRes,
        };
    }
</script>

<div class="bg-gray-900 text-gray-100 py-10 px-6 lg:px-10">
    {#await loadBasic()}
        <Loading />
    {:then res}
        <h2 class="my-2 text-lg ml-1 text-gray-300 uppercase tracking-wide">
            Weekly trends
        </h2>
        <div>            
            <PlayerbaseCard pingsWeeklyAvgs={res.pingsAvgs} tpsWeeklyAvgs={res.tpsAvg} />
        </div>
    {/await}
</div>
