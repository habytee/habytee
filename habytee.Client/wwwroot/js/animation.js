window.animateCoin = async function() {
    document.querySelector('.coin').style.animation = 'none';
    document.querySelector('.coin').offsetHeight;
    document.querySelector('.coin').style.animation = 'coin-flip 4s cubic-bezier(0, 0.2, 0.8, 1) 0s';
};
