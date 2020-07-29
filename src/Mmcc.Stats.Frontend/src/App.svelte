<script>
	import { fade } from 'svelte/transition';
	import Navbar from "./Navbar.svelte"
	import Footer from "./Footer.svelte"
	import Stats from "./Stats.svelte"
</script>

<div class="wrapper">
	<div class="cont">
		<Navbar/>
		<Stats/>
	</div>
	<Footer/>
</div>

<style>
	.wrapper {
		display: flex;
		flex-direction: column;
		min-height: 100%;
		min-width: 100%;
	}

	.cont {
		flex-grow: 1;
	}
</style>
