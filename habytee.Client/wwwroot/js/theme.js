function IsDarkMode() {
    let matched = window.matchMedia('(prefers-color-scheme: dark)').matches;

    if (matched)
    {
        return true;
    }

    return false;
}
