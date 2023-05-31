<script setup></script>

<template>
  <div class="page">
    <div class="nav">
      <RouterLink class="link" to="/">Home</RouterLink>
      <RouterLink class="link" to="/login">Login</RouterLink>
    </div>
    <div class="content">
      <RouterView />
    </div>
  </div>
</template>

<style scoped>
.nav {
  width: auto;
  height: 50px;
  text-align: center;
  position: fixed;
  top: 0;
  padding: 10px;
}
.link {
  display: inline-block;
  width: 150px;
  height: 30px;
  text-align: center;
  line-height: 30px;
  cursor: pointer;
}
</style>
