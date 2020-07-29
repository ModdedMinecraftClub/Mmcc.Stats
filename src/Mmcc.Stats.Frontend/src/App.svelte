<script>
	import Navbar from "./Navbar.svelte"
	import Footer from "./Footer.svelte"
	import Stats from "./Stats.svelte"
</script>

<wrapper>
	<main>
		<Navbar/>
		<Stats/>
	</main>
	<Footer/>
</wrapper>

<style>
	wrapper {
		display: flex;
    	flex-direction: column;
    	min-height: 100%;
	}

	main {
		flex-grow: 1;
	}
</style>