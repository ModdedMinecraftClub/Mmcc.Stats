<script lang="ts">
    import type { IMenuItemContent } from "./models";

    export let content: IMenuItemContent;
</script>

<a
    href={content.href}
    class="block mt-4 mr-4 lg:inline-block lg:mt-0 hover:text-gray-400 duration-300"
>
    {content.name}
</a>
