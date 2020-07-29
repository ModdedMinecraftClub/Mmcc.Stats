<script>
    export let playerbaseTabClass = "is-active";
    export let tpsTabClass = "";

    function handlePlayerbaseTabClick() {
        playerbaseTabClass = "is-active";
        tpsTabClass = "";
    }

    function handleTpsTabClick() {
        playerbaseTabClass = "";
        tpsTabClass = "is-active";
    }
</script>

<div id="tabs" class="tabs is-centered is-boxed is-medium">
    <ul>
        <li class="{playerbaseTabClass}" on:click="{handlePlayerbaseTabClick}"><a>Playerbase statistics</a></li>
        <li class="{tpsTabClass}" on:click="{handleTpsTabClick}"><a>TPS Statistics</a></li>
    </ul>
</div>

<style>
    #tabs {
        margin-top: 2%
    }
</style>