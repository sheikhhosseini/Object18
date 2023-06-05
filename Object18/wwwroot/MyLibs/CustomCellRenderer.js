function GenderCellRenderer(params) {
    const gender = params.cellValue;
    const imgSrc = gender ? 'man.png' : 'woman.png';

    let src = '/Images/' + imgSrc;
    return `<img class="avatar-img rounded-circle" with="80%" height="80%" src="${src}" alt="gender" />`;
}