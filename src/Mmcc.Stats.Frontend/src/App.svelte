<script lang="ts">
	import Navbar from "./navigation/Navbar.svelte";
	import DashboardMain from "./dashboard/DashboardMain.svelte";	
	import Footer from "./Footer.svelte";
</script>

<main class="flex flex-col">
	<Navbar />
	<DashboardMain />
	<Footer />
</main>

<style global>
	@tailwind base;
	@tailwind components;
	@tailwind utilities;

	html, body, main {
		@apply h-full bg-gray-900;
	}

	body {
		@apply font-body;
	}

	.card {
		@apply rounded-lg bg-gray-800 py-4 px-6 shadow-md;
	}
</style>
