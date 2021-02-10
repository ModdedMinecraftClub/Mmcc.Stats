<script lang="ts">
	import Navbar from "./navigation/Navbar.svelte";
</script>

<main>
	<Navbar />
	<div class="mt-auto h-full bg-gray-900">
		<h1 class="text-2xl text-gray-100 mt-0 pt-0">Hello!</h1>
	</div>
</main>

<style global>
	@tailwind base;
	@tailwind components;
	@tailwind utilities;

	html, body, main {
		@apply h-full;
	}

	body {
		@apply font-body;
	}
</style>
