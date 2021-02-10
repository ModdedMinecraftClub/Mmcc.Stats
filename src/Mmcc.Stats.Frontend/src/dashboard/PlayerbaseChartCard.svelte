<script lang="ts">
    interface IMode {
        id: number,
        text: string,
    };

    let modes: IMode[] = [
        { id: 0, text: "Daily rolling average" },
        { id: 1, text: "Weekly rolling average" },
        { id: 2, text: "Raw data" },
    ];
</script>

<div class="card">
    <div class="lg:px-4 lg:my-4">
        <h3 class="mb-4 text-gray-400 tracking-wide uppercase">Playerbase</h3>
        <div class="flex flex-wrap gap-3">
            <div>
                <label
                    for="from"
                    class="block font-semibold text-sm text-gray-300 ml-1 mb-0.5"
                    >From</label
                >
                <input
                    id="from"
                    name="from"
                    type="date"
                    class="border-none bg-gray-700 text-gray-100 rounded"
                />
            </div>
            <div>
                <label
                    for="to"
                    class="block font-semibold text-sm text-gray-300 ml-1 mb-0.5"
                    >To</label
                >
                <input
                    id="to"
                    name="to"
                    type="date"
                    class="border-none bg-gray-700 text-gray-100 rounded"
                />
            </div>
            <div>
                <label
                    for="mode"
                    class="block font-semibold text-sm text-gray-300 ml-1 mb-0.5"
                    >Mode</label
                >
                <select
                    id="mode"
                    name="mode"
                    class="border-none bg-gray-700 text-gray-100 rounded"
                >
                    {#each modes as mode}
                        <option value={mode}>
                            {mode.text}
                        </option>
                    {/each}
                </select>                
                <button class="px-8 lg:px-4 py-2 mt-3 lg:mt-0 lg:ml-2 bg-blue-500 text-gray-100 rounded focus:outline-none focus:ring focus:ring-blue-300">Go</button>
            </div>
        </div>
    </div>
</div>
