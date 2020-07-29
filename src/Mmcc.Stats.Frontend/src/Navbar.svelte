<script>
  document.addEventListener("DOMContentLoaded", () => {
    // Get all "navbar-burger" elements
    const navbarBurgers = Array.prototype.slice.call(
      document.querySelectorAll(".navbar-burger"),
      0
    );

    // Check if there are any navbar burgers
    if (navbarBurgers.length > 0) {
      // Add a click event on each of them
      navbarBurgers.forEach(el => {
        el.addEventListener("click", () => {
          // Get the target from the "data-target" attribute
          const target = el.dataset.target;
          const targetDom = document.getElementById(target);

          // Toggle the "is-active" class on both the "navbar-burger" and the "navbar-menu"
          el.classList.toggle("is-active");
          targetDom.classList.toggle("is-active");
        });
      });
    }
  });
</script>

<nav class="navbar is-link" role="navigation" aria-label="main navigation">
  <div class="navbar-brand">
    <a class="navbar-item" href="/">
      <img src="/img/logo_white.png" alt="MMCC Logo" />
    </a>

    <a
      role="button"
      class="navbar-burger burger"
      aria-label="menu"
      aria-expanded="false"
      data-target="navbar-b">
      <span aria-hidden="true" />
      <span aria-hidden="true" />
      <span aria-hidden="true" />
    </a>
  </div>

  <div id="navbar-b" class="navbar-menu">
    <div class="navbar-start">
      <a class="navbar-item" href="https://www.moddedminecraft.club/">
        MMCC Home
      </a>

      <a class="navbar-item" href="https://wiki.moddedminecraft.club/">Wiki</a>

      <a class="navbar-item" href="https://forum.moddedminecraft.club/">
        Forum
      </a>
    </div>

    <div class="navbar-end">
      <div class="navbar-item">
        <div class="buttons">
          <button
            class="button is-danger"
            onclick="location.href='https://www.moddedminecraft.club/store.php';">
            <span class="icon">
              <i class="fas fa-heart" />
            </span>
            <span>Donate</span>
          </button>
          <button
            class="button is-primary"
            onclick="location.href='https://discord.com/invite/8EgWdQC';">
            <span class="icon">
              <i class="fab fa-discord" />
            </span>
            <span>Discord</span>
          </button>
          <button
            class="button is-dark"
            onclick="location.href='https://github.com/ModdedMinecraftClub/Mmcc.Stats';">
            <span class="icon">
              <i class="fab fa-github" />
            </span>
            <span>GitHub</span>
          </button>
        </div>
      </div>
    </div>
  </div>
</nav>
