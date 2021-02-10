<script lang="ts">
    export let tpsAvg: number;
</script>

<div class="card">
    <h3 class="text-2xl mb-2 ml-1 text-gray-300 uppercase tracking-wide font-semibold">
        Tps
    </h3>
    <p class="font-light text-sm text-gray-300">Average TPS this week</p>
    <p
    class:text-green-500={tpsAvg >= 16}
    class:text-yellow-500={tpsAvg >= 10 && tpsAvg < 16}
    class:text-red-500={tpsAvg < 10}
    class="font-bold text-2xl w-auto">
        {tpsAvg}
    </p>
</div>
