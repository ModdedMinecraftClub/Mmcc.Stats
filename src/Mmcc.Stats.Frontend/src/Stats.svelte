<script>
    import Tabs from "./Tabs.svelte"
    import Playerbase from "./playerbase/Playerbase.svelte"
    import Tps from "./Tps.svelte"

    let playerbaseTabClass;
    let tpsTabClass;
</script>

<Tabs bind:playerbaseTabClass={playerbaseTabClass} bind:tpsTabClass={tpsTabClass}/>

{#if playerbaseTabClass === "is-active"}
    <Playerbase/>
{:else}
    <Tps/>
{/if}