<script lang="ts">
	import DashboardMain from "./dashboard/DashboardMain.svelte";
	import Footer from "./Footer.svelte";
	import Navbar from "./navigation/Navbar.svelte";
</script>

<main>
	<Navbar />
	<DashboardMain />
	<Footer />
</main>

<style global>
	@tailwind base;
	@tailwind components;
	@tailwind utilities;

	html, body {
		@apply h-full bg-gray-900;
	}

	body {
		@apply font-body;
	}

	.card {
		@apply rounded-lg bg-gray-800 py-4 px-6 shadow-md;
	}
</style>
