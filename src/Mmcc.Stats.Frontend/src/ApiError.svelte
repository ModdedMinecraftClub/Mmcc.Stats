<script lang="ts">
    import { fade } from "svelte/transition";
</script>

<p
    class="text-center mt-2"
    in:fade={{ delay: 400, duration: 300 }}
    out:fade={{ duration: 300 }}
    wmode="transparent"
>
    API error.
</p>
