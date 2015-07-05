var CC = {
prepare: function() {
  if (cur.soundCC) return;
  var scc = false, box = curBox();
  if (!window.Sound) {
    scc = cur.soundCC = {play: function () {}, pause: function() {}};
  } else {
    scc = cur.soundCC = new Sound('mp3/cc_gift_sound');
  }
  cur.destroy.push(function() {
    scc.pause();
    cur.playingCC = false;
    delete cur.soundCC;
  });
  if (box) {
    box.setOptions({onHide: function() {
      scc.pause();
      cur.playingCC = false;
      delete cur.soundCC;
    }});
  }
},
toogle: function() {
  CC.prepare();
  if (cur.playingCC) {
    cur.soundCC.pause();
    cur.playingCC = false;
  } else {
    cur.soundCC.play();
    cur.playingCC = true;
  }
  return false;
},
eof:1};
try{stManager.done('cc.js');}catch(e){}
