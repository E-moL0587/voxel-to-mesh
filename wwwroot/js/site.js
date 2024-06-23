const toggleButton = document.getElementById('toggleButton');
const toggleText = document.getElementById('toggleText');

toggleButton.addEventListener('click', () => {
  if (toggleText.style.display === 'none') {
    toggleText.style.display = 'block';
  } else {
    toggleText.style.display = 'none';
  }
});
