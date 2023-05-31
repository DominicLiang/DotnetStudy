<script setup></script>

<template>
  <RouterLink class="nav" to="/">Home</RouterLink>

  <RouterView />
</template>

<style scoped>
.nav{
  width: 100px;
  height: 50px;
  line-height: 50px;
  text-align: center;
  position: fixed;
  top: 0;
}
</style>
