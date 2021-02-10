<script lang="ts">
    import type {
        Average,
        Average2 as TpsAverage,
        Result as TpsWeeklyAvgs,
        Result2 as PingsWeeklyAvgs,
    } from "../clients";

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
            return "No data";
        } else {
            return res.avg.toFixed(2);
        }
    }
</script>

<div
    id="basic-container"
    class="card lg:flex lg:flex-col lg:h-96 lg:flex-wrap pt-56"
>
    {#each pingsWeeklyAvgs.thisWeek as avg}
        <div class="lg:mt-4 lg:px-4" id="basic-stats">
            <p class="font-semibold text-sm text-gray-300">{avg.serverName}</p>
            <div class="flex items-center">
                <div>
                    <p class="text-3xl mb-0">{avg.avg.toFixed(2)}</p>
                    <p class="font-light text-xs text-gray-500 -mt-1">
                        avg players
                    </p>
                </div>
                <div
                    class:bg-green-500={getDifferenceString(
                        avg.avg,
                        getCorrespondingLastWeek(avg)
                    ).startsWith("+")}
                    class:bg-red-500={getDifferenceString(
                        avg.avg,
                        getCorrespondingLastWeek(avg)
                    ).startsWith("-")}
                    class="rounded text-sm px-2 py-1 ml-3"
                >
                    <span class="font-bold"
                        >{getDifferenceString(
                            avg.avg,
                            getCorrespondingLastWeek(avg)
                        )}</span
                    >
                    <span class="font-light">vs last week</span>
                </div>
                <div class="border-l border-gray-500 h-8 w-1 mr-2 ml-3" />
                <div>
                    <p class="text-3xl mb-0">
                        {getCorrespondingTpsString(avg)}
                    </p>
                    <p class="font-light text-xs text-gray-500 -mt-1">
                        avg tps
                    </p>
                </div>
            </div>
        </div>
    {/each}
</div>

<style>
    #basic-stats:nth-child(4n) {
        margin-bottom: 0px !important;
    }
</style>
