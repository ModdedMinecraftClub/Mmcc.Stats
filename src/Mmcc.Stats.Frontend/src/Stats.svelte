<script>
    import { fade } from 'svelte/transition';
    import Tabs from "./Tabs.svelte"
    import PlayerbaseTab from "./PlayerbaseTab.svelte"
    import TpsTab from "./TpsTab.svelte"

    let playerbaseTabClass = "is-active";
    let tpsTabClass;
</script>

<Tabs bind:playerbaseTabClass={playerbaseTabClass} bind:tpsTabClass={tpsTabClass}/>

{#if playerbaseTabClass === "is-active"}
    <div in:fade={{delay: 400, duration: 300}} out:fade={{duration: 300}} wmode="transparent">
        <PlayerbaseTab/>
    </div>
{:else}
    <div in:fade={{delay: 400, duration: 300}} out:fade={{duration: 300}} wmode="transparent">
        <TpsTab/>
    </div>
{/if}