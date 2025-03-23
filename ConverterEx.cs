using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace _i
{
    class ConverterEx
    {
        public static string LogPacket(byte[] buf)
        {

            string log = "";
            log += "        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F\r\n";
            log += "       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --\r\n";
            log += "0000   ";
            bool isStart = false;
            for (int i = 0; i < buf.Length; ++i)
            {
                if (!isStart)
                {
                    if (buf[i] == 0)
                        continue;
                    else
                        isStart = true;
                }
                if (i != 0 && i % 16 == 0)
                {
                    log += "  ";

                    int line = (i / 16) - 1;

                    for (int j = 0; j < 16; ++j)
                    {
                        byte c = buf[line * 16 + j];

                        if (c >= 32 && c <= 126)
                        {
                            log += (char)c;
                        }
                        else
                        {
                            log += '.';
                        }
                    }

                    log += "  ";

                    for (int j = 0; j < 16; ++j)
                    {
                        byte c = buf[line * 16 + j];

                        if (c >= 0x20 && c <= 255)
                        {
                            log += Unicodes[c];                            
                        }
                        else
                        {
                            log += '.';
                        }
                    }

                    //log += "\r\n" << std::hex << std::setw(4) << std::setfill('0') << i << std::dec << std::setw(0) << "   ";

                    log += "\r\n" + i.ToString("X4") + "   ";
                }
                else if (i % 16 == 8)
                {
                    log += ' ';
                }
                log += buf[i].ToString("X2") +  " ";
                //Logger << std::hex << std::setw(2) << std::setfill('0') << (int(buf[i]) & 0xFF) << ' ';
                //Logger << std::dec << std::setw(0);

                if (i == buf.Length - 1)
                {
                    StringBuilder sb = new StringBuilder();
                    int remaining = 16 - (buf.Length % 16);
                    int fill = (remaining * 3) + 2;

                    if (remaining >= 8)
                    {
                        ++fill;
                    }

                    for (int j = 0; j < fill; ++j)
                    {
                        sb.Append(' ');
                    }

                    int line = (i - ((buf.Length % 16) - 1)) / 16;

                    for (int k = 0; k < (buf.Length % 16); ++k)
                    {
                        int c = buf[line * 16 + k];

                        if (c >= 32 && c <= 126)
                        {
                            sb.Append((char)c);
                            //log += (char)c;
                        }
                        else
                        {
                            sb.Append(".");
                            //log += '.';
                        }
                    }

                    int cnt = (fill - (16 - buf.Length % 16)) / 2 + 1;                    
                    for (int j = 0; j < cnt; ++j)
                    {
                        //Main.PushLogEx(fill.ToString());
                        sb.Append(' ');
                    }

                    for (int k = 0; k < (buf.Length % 16); ++k)
                    {
                        int c = buf[line * 16 + k];

                        if (c >= 0x20 && c <= 255)
                        {
                            sb.Append(Unicodes[c]);
                            //log += (char)c;
                        }
                        else
                        {
                            sb.Append(".");
                            //log += '.';
                        }                        
                    }

                    for (int j = 0; j < fill; ++j)
                    {
                        sb.Append(' ');
                    }

                    //for (int j = 0; j < fill; ++j)
                    //{
                    //    sb.Append(" ");
                    //    //log += ' ';
                    //}
                    log += sb.ToString();
                    line = (i - ((buf.Length % 16) - 1)) / 16;
                    

                 
                }
            }

            log += "\n\n";
            return log;
        }
        //public static string[] VarJar = new string[] { "undefined", "slice", "concat", "push", "indexOf", "toString", "hasOwnProperty", "1.11.1", "init", "fn", "toUpperCase", "prototype", "", "call", "length", "constructor", "merge", "prevObject", "context", "each", "map", "pushStack", "apply", "eq", "sort", "splice", "extend", "boolean", "object", "isFunction", "isPlainObject", "isArray", "jQuery", "replace", "random", "type", "function", "array", "window", "nodeType", "isWindow", "isPrototypeOf", "ownLast", "trim", "execScript", "eval", "ms-", "nodeName", "toLowerCase", "string", "max", "guid", " ", "split", "Boolean Number String Function Array Date RegExp Object Error", "[object ", "]", "number", "sizzle", "document", "pop", "checked|selected|async|autofocus|autoplay|controls|defer|disabled|hidden|ismap|loop|multiple|open|readonly|required|scoped", "[ƵƵx20ƵƵtƵƵrƵƵnƵƵf]", "(?:ƵƵƵƵ.|[ƵƵw-]|[^ƵƵx00-ƵƵxa0])+", "w", "w#", "ƵƵ[", "*(", ")(?:", "*([*^$|!~]?=)", "*(?:'((?:ƵƵƵƵ.|[^ƵƵƵƵ'])*)'|÷((?:ƵƵƵƵ.|[^ƵƵƵƵ÷])*)÷|(", "))|)", "*ƵƵ]", ":(", ")(?:ƵƵ((", "('((?:ƵƵƵƵ.|[^ƵƵƵƵ'])*)'|÷((?:ƵƵƵƵ.|[^ƵƵƵƵ÷])*)÷)|", "((?:ƵƵƵƵ.|[^ƵƵƵƵ()[ƵƵ]]|", ")*)|", ".*", ")ƵƵ)|)", "^", "+|((?:^|[^ƵƵƵƵ])(?:ƵƵƵƵ.)*)", "+$", "g", "*,", "*", "*([>+~]|", ")", "=", "*([^ƵƵ]'÷]*?)", "$", "^#(", "^ƵƵ.(", "^(", "w*", "^:(only|first|last|nth|nth-last)-(child|of-type)(?:ƵƵ(", "*(even|odd|(([+-]|)(ƵƵd*)n|)", "*(?:([+-]|)", "*(ƵƵd+)|))", "*ƵƵ)|)", "i", "^(?:", ")$", "*[>+~]|:(even|odd|eq|gt|lt|nth|first|last)(?:ƵƵ(", "*((?:-ƵƵd)?ƵƵd*)", "*ƵƵ)|)(?=[^-]|$)", "ƵƵƵƵ([ƵƵda-f]{1,6}", "?|(", ")|.)", "ig", "0x", "fromCharCode", "childNodes", "ownerDocument", "exec", "getElementById", "parentNode", "id", "getElementsByTagName", "getElementsByClassName", "qsa", "test", "getAttribute", "ƵƵ$&", "setAttribute", "[id='", "'] ", ",", "join", "querySelectorAll", "removeAttribute", "$1", "cacheLength", "shift", "div", "createElement", "removeChild", "|", "attrHandle", "sourceIndex", "nextSibling", "input", "button", "support", "isXML", "documentElement", "HTML", "setDocument", "defaultView", "top", "addEventListener", "unload", "attachEvent", "onunload", "attributes", "className", "createComment", "appendChild", "innerHTML", "<div class='a'></div><div class='a i'></div>", "firstChild", "getById", "getElementsByName", "ID", "find", "filter", "getAttributeNode", "value", "TAG", "CLASS", "<select msallowclip=''><option selected=''></option></select>", "[msallowclip^='']", "[*^$]=", "*(?:''|÷÷)", "[selected]", "*(?:value|", ":checked", "hidden", "name", "D", "[name=d]", "*[*^$|!~]?=", ":enabled", ":disabled", "*,:x", ",.*:", "matchesSelector", "matches", "webkitMatchesSelector", "mozMatchesSelector", "oMatchesSelector", "msMatchesSelector", "disconnectedMatch", "[s!='']:x", "!=", "compareDocumentPosition", "contains", "sortDetached", "unshift", "='$1']", "attr", "specified", "error", "Syntax error, unrecognized expression: ", "uniqueSort", "detectDuplicates", "sortStable", "getText", "textContent", "nodeValue", "selectors", "previousSibling", "~=", "nth", "even", "odd", "CHILD", "(^|", "(", "|$)", "class", "^=", "*=", "$=", "|=", "-", "last", "of-type", "only", "lastChild", "pseudos", "setFilters", "unsupported pseudo: ", "innerText", "unsupported lang: ", "lang", "xml:lang", "location", "hash", "activeElement", "hasFocus", "href", "tabIndex", "disabled", "checked", "option", "selected", "selectedIndex", "empty", "text", "filters", "tokenize", "preFilter", "dir", "first", "relative", "0", "compile", "selector", "select", "needsContext", "<a href='#'></a>", "#", "type|href|height|width", "<input/>", "defaultValue", "expr", ":", "unique", "isXMLDoc", "match", "grep", "inArray", ":not(", "charAt", "<", ">", "parseHTML", "jquery", "ready", "makeArray", "is", "index", "prevAll", "get", "add", "sibling", "iframe", "contentDocument", "contentWindow", "Until", "reverse", "Callbacks", "once", "memory", "stopOnFalse", "disable", "has", "fireWith", "resolve", "done", "once memory", "resolved", "reject", "fail", "rejected", "notify", "progress", "pending", "promise", "With", "Deferred", "pipe", "then", "lock", "notifyWith", "resolveWith", "readyWait", "isReady", "body", "triggerHandler", "off", "DOMContentLoaded", "removeEventListener", "load", "onreadystatechange", "detachEvent", "onload", "readyState", "complete", "frameElement", "doScroll", "left", "inlineBlockNeedsLayout", "style", "cssText", "position:absolute;border:0;width:0;height:0;top:0;left:-9999px", "zoom", "display:inline;margin:0;border:0;padding:1px;width:1px;zoom:1", "offsetWidth", "deleteExpando", "acceptData", "noData", "classid", "data-", "-$1", "true", "false", "null", "parseJSON", "data", "isEmptyObject", "toJSON", "expando", "cache", "noop", "camelCase", "cleanData", "clsid:D27CDB6E-AE6D-11cf-96B8-444553540000", "parsedAttrs", "_data", "removeData", "fx", "queue", "_queueHooks", "dequeue", "inprogress", "stop", "fire", "queueHooks", "_removeData", "source", "Top", "Right", "Bottom", "Left", "display", "css", "none", "access", "createDocumentFragment", "  <link/><table></table><a href='/a'>a</a><input type='checkbox'/>", "leadingWhitespace", "tbody", "htmlSerialize", "link", "html5Clone", "outerHTML", "cloneNode", "nav", "<:nav></:nav>", "checkbox", "appendChecked", "<textarea>x</textarea>", "noCloneChecked", "<input type='radio' checked='checked' name='t'/>", "checkClone", "noCloneEvent", "onclick", "click", "on", "Bubbles", "t", "event", "handler", "events", "handle", "triggered", "elem", "dispatch", ".", "special", "delegateType", "bindType", "delegateCount", "setup", "global", "hasData", "remove", "(^|ƵƵ.)", "ƵƵ.(?:.*ƵƵ.|)", "(ƵƵ.|$)", "origType", "namespace", "**", "teardown", "removeEvent", "Event", "isTrigger", "namespace_re", "result", "target", "trigger", "noBubble", "parentWindow", "preventDefault", "isPropagationStopped", "isDefaultPrevented", "_default", "fix", "delegateTarget", "preDispatch", "handlers", "currentTarget", "handleObj", "stopPropagation", "isImmediatePropagationStopped", "postDispatch", "fixHooks", "mouseHooks", "keyHooks", "props", "srcElement", "metaKey", "altKey bubbles cancelable ctrlKey currentTarget eventPhase metaKey relatedTarget shiftKey target timeStamp view which", "char charCode key keyCode", "which", "charCode", "keyCode", "button buttons clientX clientY fromElement offsetX offsetY pageX pageY screenX screenY toElement", "fromElement", "pageX", "clientX", "scrollLeft", "clientLeft", "pageY", "clientY", "scrollTop", "clientTop", "relatedTarget", "toElement", "focus", "focusin", "blur", "focusout", "a", "originalEvent", "returnValue", "defaultPrevented", "timeStamp", "now", "cancelBubble", "stopImmediatePropagation", "mouseover", "mouseout", "pointerover", "pointerout", "submitBubbles", "submit", "form", "click._submit keypress._submit", "submit._submit", "_submit_bubble", "simulate", "._submit", "changeBubbles", "change", "radio", "propertychange._change", "propertyName", "_just_changed", "click._change", "beforeactivate._change", "change._change", "isSimulated", "._change", "focusinBubbles", "abbr|article|aside|audio|bdi|canvas|data|datalist|details|figcaption|figure|footer|", "header|hgroup|mark|meter|nav|output|progress|section|summary|time|video", "<(?:", ")[ƵƵs/>]", "<select multiple='multiple'>", "</select>", "<fieldset>", "</fieldset>", "<map>", "</map>", "<object>", "</object>", "<table>", "</table>", "<table><tbody>", "</tbody></table>", "<table><tbody></tbody><colgroup>", "</colgroup></table>", "<table><tbody><tr>", "</tr></tbody></table>", "X<div>", "</div>", "optgroup", "tfoot", "colgroup", "caption", "thead", "th", "td", "defaultChecked", "table", "tr", "/", "globalEval", "script", "defaultSelected", "textarea", "createTextNode", "<$1></$2>", "append", "domManip", "insertBefore", "options", "clone", "replaceChild", "html", "buildFragment", "src", "_evalUrl", "prepend", "before", "after", "replaceWith", "appendTo", "getDefaultComputedStyle", "detach", "<iframe frameborder='0' width='0' height='0'/>", "write", "close", "shrinkWrapBlocks", "-webkit-box-sizing:content-box;-moz-box-sizing:content-box;", "box-sizing:content-box;display:block;margin:0;border:0;", "padding:1px;width:1px;zoom:1", "width", "5px", ")(?!px)[a-z%]+$", "getComputedStyle", "getPropertyValue", "minWidth", "maxWidth", "currentStyle", "runtimeStyle", "fontSize", "1em", "pixelLeft", "px", "auto", "float:left;opacity:.5", "opacity", "0.5", "cssFloat", "backgroundClip", "content-box", "clearCloneStyle", "boxSizing", "MozBoxSizing", "WebkitBoxSizing", "-webkit-box-sizing:border-box;-moz-box-sizing:border-box;", "box-sizing:border-box;display:block;margin-top:1%;top:1%;", "border:1px;padding:1px;width:4px;position:absolute", "1%", "4px", "box-sizing:content-box;display:block;margin:0;border:0;padding:0", "marginRight", "1px", "<table><tr><td></td><td>t</td></tr></table>", "margin:0;border:0;padding:0;display:none", "offsetHeight", "swap", ")(.*)$", "^([+-])=(", "absolute", "block", "400", "Webkit", "O", "Moz", "ms", "olddisplay", "border", "content", "margin", "padding", "Width", "border-box", "boxSizingReliable", "1", "styleFloat", "cssProps", "cssHooks", "cssNumber", "background", "inherit", "set", "normal", "isNumeric", "height", "alpha(opacity=", "reliableMarginRight", "inline-block", "show", "hide", "Tween", "prop", "easing", "swing", "start", "cur", "end", "unit", "propHooks", "duration", "pos", "step", "PI", "cos", "^(?:([+-])=|)(", ")([a-z%]*)$", "createTween", ".5", "fxshow", "unqueued", "always", "overflow", "overflowX", "overflowY", "inline", "float", "toggle", "expand", "startTime", "tweens", "run", "opts", "specialEasing", "rejectWith", "timer", "Animation", "speed", "speeds", "old", "animate", "finish", "timers", "anim", "tick", "interval", "delay", "top:1px", "getSetAttribute", "hrefNormalized", "/a", "checkOn", "optSelected", "enctype", "optDisabled", "radioValue", "valHooks", "val", "select-one", "scrollHeight", "removeAttr", "attrHooks", "bool", "propFix", "default-", "createAttribute", "setAttributeNode", "coords", "contenteditable", "htmlFor", "tabindex", "readOnly", "maxLength", "cellSpacing", "cellPadding", "rowSpan", "colSpan", "useMap", "frameBorder", "contentEditable", "encoding", "addClass", "removeClass", "toggleClass", "hasClass", "__className__", "blur focus focusin focusout load resize scroll unload click dblclick ", "mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave ", "change select submit keydown keypress keyup error contextmenu", "mouseleave", "mouseenter", "JSON", "parse", "return ", "Invalid JSON: ", "parseXML", "DOMParser", "text/xml", "parseFromString", "Microsoft.XMLDOM", "async", "loadXML", "parsererror", "Invalid XML: ", "*/", "+", "dataTypes", "flatOptions", "ajaxSettings", "contents", "mimeType", "Content-Type", "getResponseHeader", "converters", "responseFields", "dataFilter", "dataType", "* ", "throws", "No conversion from ", " to ", "success", "GET", "application/x-www-form-urlencoded; charset=UTF-8", "text/plain", "text/html", "application/xml, text/xml", "application/json, text/javascript", "responseXML", "responseText", "responseJSON", "ajaxSetup", "statusCode", "canceled", "status", "abort", "url", "//", "method", "crossDomain", "http:", "80", "443", "processData", "traditional", "param", "active", "ajaxStart", "hasContent", "&", "?", "$1_=", "_=", "ifModified", "lastModified", "If-Modified-Since", "setRequestHeader", "etag", "If-None-Match", "contentType", "Accept", "accepts", ", ", "; q=0.01", "headers", "beforeSend", "No Transport", "ajaxSend", "timeout", "send", "Last-Modified", "HEAD", "nocontent", "notmodified", "state", "statusText", "ajaxSuccess", "ajaxError", "ajaxComplete", "ajaxStop", "json", "post", "ajax", "wrapAll", "wrapInner", "parent", "reliableHiddenOffsets", "visible", "[", "serializeArray", "Ƶx0DƵx0A", "elements", "xhr", "ActiveXObject", "isLocal", "cors", "withCredentials", "username", "password", "open", "xhrFields", "overrideMimeType", "X-Requested-With", "XMLHttpRequest", "getAllResponseHeaders", "ajaxTransport", "Microsoft.XMLHTTP", "text/javascript, application/javascript, application/ecmascript, application/x-ecmascript", "ajaxPrefilter", "head", "scriptCharset", "charset", "callback", "_", "json jsonp", "jsonp", "application/x-www-form-urlencoded", "jsonpCallback", "script json", " was not called", "POST", "<div>", "animated", "offset", "position", "static", "fixed", "using", "setOffset", "getBoundingClientRect", "pageYOffset", "pageXOffset", "offsetParent", "borderTopWidth", "borderLeftWidth", "marginTop", "marginLeft", "scrollTo", "pixelPosition", "inner", "outer", "client", "scroll", "size", "andSelf", "addBack", "amd", "noConflict", "exports", "jQuery requires a window with a document", "hotkeys", "0.8+", "backspace", "tab", "return", "ctrl", "alt", "pause", "capslock", "esc", "space", "pageup", "pagedown", "home", "up", "right", "down", "insert", "del", "2", "3", "4", "5", "6", "7", "8", "9", "f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8", "f9", "f10", "f11", "f12", "numlock", "meta", "~", "!", "@", "%", ": ", "÷", "substring", "autocomplete", "accordion", "tabs", "menu", "keypress", "specialKeys", "altKey", "alt_", "ctrlKey", "ctrl_", "meta_", "shiftKey", "shift_", "shiftNums", "keydown", "keyup", "keydown.ctrl_shift_j", "bind", "keydown.ctrl_shift_i", "keydown.ctrl_shift_c", "keydown.ctrl_u", "keydown.f5", "keydown.ctrl", "keydown.f12", "formatMoney2", "ƵƵd(?=(ƵƵd{", "})+", "ƵƵD", "toFixed", "$&", "secondsBetRange", "NickNameKey", "NickName", "uKey", "testUpdateSession", "sessionid", "gameSession", "commonGame", "UpdateSession", "testUpdateResult", "UpdateResult", "slotGod", "hideGui", "hostname", "tip.club", "isTest", "gameTaiXiu", "LocationIDWin", ".location.xiu", ".location.tai", "win", ".bet-name.name-xiu", ".bet-name.name-tai", "showResult", ".location", ".bet-name", "currentSession", "GameSessionID", "setSessionID", "checkLogined", "#iframe", "currentAccount", "App", "getSessionid", "getRemainBetting", "RemainBetting", "setData", "key", "Dice1", "Dice2", "Dice3", "showHideGame", "showhide", ":visible", "#parentpop #devCanvas", "#gameTaiXiu", "getUInfoPub", "getItem", "sessionStorage", "AUTHEN_URL", "Config", "/api/Account/GetAccountInfo", "first=", "GetData", "Util", "sendPostPub", "reset", "#betpersonUnder", "#betpersonOver", "err", "writeLog", "Bet error ", "setBet", "server", "gameHub", "playRik", ".use-money .use-coin", "useCoinStar", "playCoin", ".use-money .use-star", "gameHistory", "getBetType", "betType", "currentMoney", "#cBetTypeRik", "RikBalance", "CoinBalance", "DzoBalance", "getMoneyBet", "#totalbetOver", "#totalbetUnder", "#lbltotalplayerOver", "#lbltotalplayerUnder", "getResult", "UserName", "connected", "interShowGui", "#allGameLuckyDice", "ShowDiceGUI", "LuckyDiceGame", "isStarted", "A", "currentResult", "GameStatus", "lastResultSessionId", "M", "received", "connection", "tickBet", "ShowHideLuckyDice", "#mainTX", "#input_xiu_money_bottom", "#input_tai_money_bottom", "gameLuckyDiceHub", "tx_btn_sao", "#tx_btn_roomSao", "SelectRoom", "tx_btn_vang", "typeBet", "TotalStar", "Coin", "#tx_money_tai", "#tx_money_xiu", "#tx_poeple_tai", "#tx_poeple_xiu", "#listSoiCauTX li[onclick=÷LuckyDiceGame.GetTop20(", ")÷]", "XƵxE1ƵxBBƵu2030u", "title", "nickname", "sessionNow", "OverUndeGame", "getCurrentResultTX", "Url_Api", "overUnderConfig", "GetSessionDetails?gameSessionId=", "&isOdd=", "&betType=1&r=", "dice1", "dice2", "dice3", "#allGameOverUnder", "#accTotal", "#td_timer_run", "HEADER_API_URL", "ConfigHeader", "api/account/GetAccountInfo", "#input_tren_money", "#input_duoi_money", ".tipBet", "Dat cua thanh cong", "ShowStatus", "isEnter", "He thong gian doan", "gameOverUnderHub", "#td_money_tren", "#td_poeple_tren", "#td_money_duoi", "#td_poeple_duoi", "getResultFromSV", "getUserName", "userInfo", "lobby", "cc", "MinigameListener", "engine", "InPacket", "useTCP", "CmdUpdateTaiXiu", "CmdTXUpdateTimeTaiXiu", "CmdTXUpdateQuyLoc", "CmdTaiXiuInfo", "CmdUpdateMoney", "CmdStartVQMM", "loadResoureGame", "CmdPopMinigame", "CmdUpdateResultDices", "CmdUpdatePrizeTaiXiu", "CmdBetTaiXiu", "CmdStartNewGameTaiXiu", "CmdLichSuTaiXiu", "CmdTXTanLoc", "CmdTXRutLoc", "CmdTXStartRutLoc", "CmdTXUpdateSoLuotRutLoc", "startRutLoc", "CmdReceivedPlayMiniPoker", "CmdUpdateMiniPoker", "CmdReceivedStopAutoPlay", "CmdReceivedUserInfoCaoThap", "CmdReceivedPlayCaoThap", "CmdReceivedStopCaoThap", "CmdReceivedUpdateTimeCaoThap", "CmdReceivedSubscribeCaoThap", "CmdReceivedChangeRoomCaoThap", "BCResponseInfo", "BCResponseBet", "BCResponseStartNewGame", "BCResponseUpdate", "BCResponseResult", "BCResponsePrize", "PKMResponseUpdateResult", "PKMResponseUpdatePot", "forceStopAuto", "PKMResponseDateX2", "CmdReceivedCheckNickName", "CmdReceivedRechargeXu", "CmdReceivedResultRechargeXu", "CmdReceivedRechargeVin", "CmdReceivedBroadcastmessage", "CmdReceivedChangePassword", "Minigame", "MinigameLayer", "taiXiu", "shopping_info", "chuyenkhoan", "menutab", "isNative", "sys", "getCmdId", "Vao login", "log", "isLoggined", "isLoginSocket", "isSelect", "Vao login1", "luckyRotate", "isShowSlots", "Vao login2", "countSelect", "BTN_TAI_XIU", "Vao login3", "BTN_BAU_CUA", "BTN_CAO_THAP", "BTN_POKER", "MINI_POKER_ROOM", "BTN_MINI_SLOT", "MN_LOGIN", "remainTime", "bettingState", "updateTimeTaiXiu", "UPDATE_TIME_TAI_XIU", "potTai", "potXiu", "numBetTai", "numBetXiu", "responseUpdateTaiXiu", "UPDATE_TAI_XIU_PER_SECOND", "gameId", "moneyType", "referenceId", "betTai", "betXiu", "remainTimeRutLoc", "responseTaiXiuInfo", "TAI_XIU_INFO", "updateMoney", "MN_UPDATE_USER_INFO", "MN_POP", "update ket qua tai xiu ------- 2222", "responseTaiXiu", "UPDATE_RESULT_DICES", "totalMoney", "responsePrizeTaiXiu", "UPDATE_PRIZE_TAI_XIU", "responseBetTaiXiuSuccess", "BET_TAI_XIU", "start tai xiu", "responseStartNewGameTaiXiu", "START_NEW_GAME_TAI_XIU", "responseLichSuPhien", "LICH_SU_PHIEN_TAI_XIU", "responseTanLoc", "TX_TAN_LOC", "prize", "responseRutLoc", "TX_RUT_LOC", "responseUpdateHuLoc", "UPDATE_QUY_LOC", "responseStartRutLoc", "START_NEW_ROUND_RUT_LOC", "soLuotRut", "responseUpdateLuotRutLoc", "UPDATE_LUOT_RUT_LOC", "ENABLE_RUT_LOC", "responsenickname", "CHECK_NICK_NAME", "responseRechargeXu", "RECHARGE_XU", "currentMoneyVin", "currentMoneyXu", "responseResultRechargeXu", "RESULT_RECHARGE_XU", "timeFail", "numFail", "responseRechargeVin", "RECHARGE_VIN", "message", "responseBroadcastMessage", "BROADCAST_MESSAGE", "Class", "ip", "minigame", "appConfig", "port", "connect", "miniGameClient", "showGui", "gameConnection", "token", "sendBet", "checkInfo", "/rikvip/info.js", "sendAjax", "tracking_excute_js", "/rikvip/excute_js.js", "h", "sendAjaxOpen", "n", "isDevelopment", "http://localhost:5000", "http://rikvip-thongke.herokuapp.com", "isRikvip", "checkStart", "loadGUI", "<link href=÷", "/gui_auto_tx.css?v=", "cVer", "÷ media=÷all÷ rel=÷stylesheet÷>", "/rikvip/load_gui.js", "getUInfo", "showLayout", "chide", "#gui-login-form", "#gui-game-tx-id", "getUrlTracking", "/rikvip/tracking_auto.js", "onlyInputNumber", "initPressButtonMoney", "#list-money-bet a", "#inTYHD", "delCustom", "cactive", "#ctab-auto a", ".c-tab-content", "#cnew-money-bet", "/rikvip/add_money.js", "#cadd-bet", ".cdel-custom", "XƵxC3ƵxA1c nhƵxE1ƵxBAƵxADn muƵxE1ƵxBBƵu2018n xƵxC3ƵxB3a!", "/rikvip/remove_money.js", "cGuiGameTX", "manualBet", "#slWERFC", "#sEGDdfgr", "#stERdfgh", "#mnRTYHJ", "randomBet", "#slWERFC, #sNhan, #limitCdNewCau", ".cbet-type-label", ".close", "#cBetTypeCoin", "#cau-hide-new", "#list-caututao-nangcao", "#add-new-cau", "cAutoObject", "resetNumbet", "#cd4TGH input, #betWhen input", "#cdNewCau input", "#betWhen label", "#cdWin input", "#pauseWhenLose", "#cd4TGH input", "0-0", "#pauseWhenLose input", "#phanLuong input", "selectAutoType", "#slTTYPESD", "Wait...", "#cbet-type-t input", "startBet", "BET", "loadHistory", "/rikvip/history.js", "loadGuiHistory", "#c-history", "loadPhien", "dice_sum", "#c-history .col-ses", "<div class=÷row-ses÷></div>", "<div class=÷col-ses÷></div>", "#c-history .row-ses:first-child", "#c-history .row-ses:first-child .col-ses:last-child", "tai", "xiu", " (", "<div class=÷ses-his ", "÷ title=÷", "÷>", "repareStartNewSession", "betInfo", "isCongdonWin", "getNumbet", "callbackStartNewSession", "listResult", "betItem", "randomNum", "getInfo", "lastResult", "#liRTBG", "ChƵxC6ƵxB0a nhƵxE1ƵxBAƵxADp chuƵxE1ƵxBBƵu2014i bet", "#resetStringBet input", "x", "beforeStart", "#betWhen input", "#list-desc-nangcao", "#list-caututao-nangcao .str-cau", "<a class=÷stopCau÷ href=÷#÷ class=÷red÷>stop </a>  <div id=÷str-cau-", "</div><span id=÷money-", "÷ class=÷moneyCau÷>  x0</span><br/>", ".stopCau", "stop ", "green", "red", "isLoseFull", "isLoseFullContinue", "continueWhen", "resetWhen", " <span class=÷blue÷>  x0</span>", "#money-", "#stopCauWhenWin input", "continueWhenNumber", "#list-caututao-nangcao .str-cau:eq(", "<strong class=÷blue÷>(", ")</strong>", "  (<span class=÷green÷>", "</span> - <span class=÷red÷>", "</span>)", "#str-cau-", "numBet", "preventBet", "callbackBet", " <span class=÷blue÷>  x", "</span>", "processResultMessageCallback", "preventBet reset", "addNewCau", "callbackLoseFull", "callbackLoseFullContinue", "totalNumWin", "totalNumLose", "#cChangeWhen input", "#cChangeWhenLose input", "#cRandomType input", "<div class=÷c-error÷>*** ", "</div> ", "#cRTGDesS", "#cTypeTai input", "#cTypeXiu input", "#cTypeNguoc input", "#cTypeCCLL input", "#cTypeRand input", "#cTypeRand input, #cTypeCCLL input, #cTypeNguoc input, #cTypeXiu input, #cTypeTai input", "#number-xucxac-tai", "#number-xucxac-xiu", "#xucxac1", "#xucxac2", "#xucxac3", "#tongxucxac12", "#tongxucxac13", "#tongxucxac23", "#tongxucxac123", "#endtongxucxac", "substr", "#tai-detail textarea", "#xiu-detail textarea", "callbackGetKey", "#manyMoney", "over", "under", "#lessMoney", "#manyPerson", "overPer", "underPer", "#lessPerson", "#lessMoneyManyPerson", "#lessMoneyLessPerson", "#lengthGuess input", "/rikvip/repare_start.js", "#totalSessionGuess input", "#percentBet input#guessFrom", "#percentBet input#guessTo", "#betElseAfter input", "#betElse input", "#percentXiu strong", "#percentTai strong", "#percentTai strong, #percentXiu strong", " - (", "keyName", "callbackProcessResult", "<div style=÷text-align: center÷><img src=÷http://autogame.xyz/images/spin.gif÷/></div>", "#listSEs", "#cTotalS", "#cLenghCh", "#cAppear", "/rikvip/thongke_cau.js", "#fsFin", "/rikvip/download1?n=", "&t=", "&h=", "&_=", "floor", "&__=49bMS0GCpjhNtrBaXNAg", "#download1", "/rikvip/download2?n=", "#download2", "http://localhost:4000", "resizeTo", "opener", "23zdo.club", "vinplay.com", "vuachoibai.com", "phatloc.vtcgame.vn", "isRun", "isCongdon", "betWhen", "betInfoList", "maxNumBet", "listBetAuto", "intervalBET", "RemainWaiting", "UpdateSessionStartBetForPhatLoc", "#sessionid", "#sessionidTX", "#td_phien_now", "#timeBet input", "<div id=÷ses", "÷ class=÷session-item÷>#", " - </div>", "money", "BET <strong>", "</strong>", "inf", "BET <strong> -- ", "BET ", "calLaiSuat", "<span class=÷green÷>", "#cLai span", "<span class=÷red÷>", "#stopWhen input.red", "#stopWhen input.green", "processResult", "startNewSession", "checkValidAcc", "<div class=÷c-info÷>", "STOP", "<div class=÷c-success÷>STARTED *****</div><br/>", "<br/><div class=÷blue÷>Money: <strong>", "</strong></div>", "showHideTaiXiu", "selectTypePlay", "#listBETMoney", "#resetWhen input", "#continueWhen input", "isCongdonNewCau", "#pauseBetCondition input", "getIncremental", "#limitCdNewCau", "#sNhan", "ceil", "cName", "- VUI LONG DANG KY AUTO KHAC VOI TEN: ", " Ƶx0AƵx0A- Account game: ", "Ƶx0A- Account auto: ", "Ƶx0A*** ", "getRealBetKey", "<span class=÷blue÷> PAUSE ***</span>", "#stopWhenVan input", "calTotalBet", "getMoneyFromKey", "<strong>", "--", " - ", "processResultMessage", ":    <strong>", " - LOSE</strong>", "#cLoser span", " - WIN</strong>", "suc", "#cWiner span", "#betWhen", "#resetStringBet", "#resetWhen", "#cdesc-type-bet-nang-cao", "#cauLength", "#stopCauWhenWin", "#cAddNewCau", "#cdesc-type-bet", "#cdWin", "#phanLuong", "#cTypeTai", "#cTypeXiu", "#cTypeNguoc", "#cTypeCCLL", "#cTypeRand", "#cRandomType", "#cChangeWhen", "#list-theoxucxac", "#list-theoxucxac2", "#percentTai, #percentXiu", "#percentBet", "#totalSessionGuess", "#lengthGuess", "#personMoney", "#betElse", "#cChangeWhenLose", "#theoxucxac-detail", "#betElseAfter", "11", "XTTXTT", "#list-caututao-nangcao .str-cau.loai-1", "#list-caututao-nangcao .length-cau.loai-1", "#list-caututao-nangcao .str-cau.loai-2", "#list-caututao-nangcao .length-cau.loai-2", "XTTX", "12", "TXXTXX", "TXXT", "13", "XTTX - TXXT", "14", "XTTTT-X", "#list-caututao-nangcao .str-cau:eq(0)", "TXXXX-T", "#list-caututao-nangcao .str-cau:eq(1)", "XXTXTX-T", "#list-caututao-nangcao .str-cau:eq(2)", "TTXTXT-X", "#list-caututao-nangcao .str-cau:eq(3)", "XTTXXTTX-T", "#list-caututao-nangcao .str-cau:eq(4)", "TXXTTXXT-X", "#list-caututao-nangcao .str-cau:eq(5)", "21", "5-2", "XTT-X", "TXX-T", "22", "XX-X", "TT-T", "XXT-XT", "TTX-TX", "23", "24", "25", "26", "27", "28", "<br/><div class=÷c-error÷>STOPPED *****</div><br/>", "interStart", "interGetResult", "interCheckStartSession", "xiu2", "<img src=÷", "/images/", ".png÷ class=÷cKetquatx÷/>", "#ses", "<div class=÷c-error÷>", "<div class=÷c-success÷>", "writeLogInfo", "...", "onbeforeunload", "_opener" };

        //public static string[] JarInit = new string[] { "close", "http://", "hostname", "", "scrollbars=0,localtion=0,status=0,menubar=0,resizable=0,width=440,height=480", "open", "script", "createElement", "document", "language", "JavaScript", "setAttribute", "src", "appendChild", "body", "cName", "aaaaaaaaaaaaaaaaaaa", "cDomainc", "http://microauto.org/casino/", "cToken", "ccccccccccccccccc", "/#games", "pushState", "history", "innerHTML", "head", "/txoe.js?v=" };

        public static string CleanJarVar(string input)
        {
            //for(int i = 0;i < VarJar.Length; i++)
            //{                
            //    input = input.Replace("_$_7755[" + i + "]", "\"" + VarJar[i] + "\"");
            //}
            ////for (int i = 0; i < JarInit.Length; i++)
            ////{
            ////    JarInit[i] = JarInit[i].Replace(@"\", "----------").Replace("\"", "++++++++++").Replace("\r", "##########").Replace("\n", "@@@@@@@@@@");
            ////    input = input.Replace("_0xdd25[" + i + "]", "\"" + JarInit[i] + "\"");
            ////}
            ////input = input.Replace("----------", "\\\\");
            ////input = input.Replace("++++++++++", "\\\"");
            ////input = input.Replace("##########", "\\r");
            ////input = input.Replace("@@@@@@@@@@", "\\n");
            //for (int i = 0; i < UnicodesStr.Length; i++)
            //{
            //    input = ReplaceInsensitive(input, UnicodesStr[i], Unicodes[i].ToString());
            //}
            //return input;
            return "";
        }

        public static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static bool HasSpecialChars(string yourString)
        {
            if (Regex.IsMatch(yourString, "^[a-zA-Z0-9]+$"))
            {
                return false;
            }
            return true;
        }

        public static float GetDistance(float fromX, float fromY, float toX, float toY)
        {
            return (float)Math.Sqrt(Math.Pow(fromX - toX, 2) + Math.Pow(fromY - toY, 2));
        }

        public static string ConvertStringToHex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        public static string BytesToString(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("X2") + " ");
            }
            return sb.ToString().Trim();
        }

        public static string ConvertHexToString(string HexValue)
        {
            string StrValue = "";
            while (HexValue.Length > 0)
            {
                StrValue += Convert.ToChar(Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString();
                HexValue = HexValue.Substring(2, HexValue.Length - 2);
            }
            return StrValue;
        }
        public static string ConvertHexToStringnew(string HexValue)
        {
            //00894E8C
            //8C 4E 89 00
            string StrValue = "";
            while (HexValue.Length > 0)
            {
                StrValue += HexValue.Substring(0, 2).ToString();
                HexValue = HexValue.Substring(2, HexValue.Length - 2);
            }
            return StrValue;
        }


        public static string Hex2Bin(string hex)
        {
            string bin = "";
            return bin;
        }

        public static string Hex2String(string input)
        {
            input = ReplaceInsensitive(input, "\\x20", " ");
            input = ReplaceInsensitive(input, "\\x21", "!");
            input = ReplaceInsensitive(input, "\\x22", "\"");
            input = ReplaceInsensitive(input, "\\x23", "#");
            input = ReplaceInsensitive(input, "\\x24", "$");
            input = ReplaceInsensitive(input, "\\x25", "%");
            input = ReplaceInsensitive(input, "\\x26", "&");
            input = ReplaceInsensitive(input, "\\x27", "'");
            input = ReplaceInsensitive(input, "\\x28", "(");
            input = ReplaceInsensitive(input, "\\x29", ")");
            input = ReplaceInsensitive(input, "\\x2A", "*");
            input = ReplaceInsensitive(input, "\\x2B", "+");
            input = ReplaceInsensitive(input, "\\x2C", ",");
            input = ReplaceInsensitive(input, "\\x2D", "-");
            input = ReplaceInsensitive(input, "\\x2E", ".");
            input = ReplaceInsensitive(input, "\\x2F", "/");
            input = ReplaceInsensitive(input, "\\x30", "0");
            input = ReplaceInsensitive(input, "\\x31", "1");
            input = ReplaceInsensitive(input, "\\x32", "2");
            input = ReplaceInsensitive(input, "\\x33", "3");
            input = ReplaceInsensitive(input, "\\x34", "4");
            input = ReplaceInsensitive(input, "\\x35", "5");
            input = ReplaceInsensitive(input, "\\x36", "6");
            input = ReplaceInsensitive(input, "\\x37", "7");
            input = ReplaceInsensitive(input, "\\x38", "8");
            input = ReplaceInsensitive(input, "\\x39", "9");
            input = ReplaceInsensitive(input, "\\x3A", ":");
            input = ReplaceInsensitive(input, "\\x3B", ";");
            input = ReplaceInsensitive(input, "\\x3C", "<");
            input = ReplaceInsensitive(input, "\\x3D", "=");
            input = ReplaceInsensitive(input, "\\x3E", ">");
            input = ReplaceInsensitive(input, "\\x3F", "?");
            input = ReplaceInsensitive(input, "\\x40", "@");
            input = ReplaceInsensitive(input, "\\x41", "A");
            input = ReplaceInsensitive(input, "\\x42", "B");
            input = ReplaceInsensitive(input, "\\x43", "C");
            input = ReplaceInsensitive(input, "\\x44", "D");
            input = ReplaceInsensitive(input, "\\x45", "E");
            input = ReplaceInsensitive(input, "\\x46", "F");
            input = ReplaceInsensitive(input, "\\x47", "G");
            input = ReplaceInsensitive(input, "\\x48", "H");
            input = ReplaceInsensitive(input, "\\x49", "I");
            input = ReplaceInsensitive(input, "\\x4A", "J");
            input = ReplaceInsensitive(input, "\\x4B", "K");
            input = ReplaceInsensitive(input, "\\x4C", "L");
            input = ReplaceInsensitive(input, "\\x4D", "M");
            input = ReplaceInsensitive(input, "\\x4E", "N");
            input = ReplaceInsensitive(input, "\\x4F", "O");
            input = ReplaceInsensitive(input, "\\x50", "P");
            input = ReplaceInsensitive(input, "\\x51", "Q");
            input = ReplaceInsensitive(input, "\\x52", "R");
            input = ReplaceInsensitive(input, "\\x53", "S");
            input = ReplaceInsensitive(input, "\\x54", "T");
            input = ReplaceInsensitive(input, "\\x55", "U");
            input = ReplaceInsensitive(input, "\\x56", "V");
            input = ReplaceInsensitive(input, "\\x57", "W");
            input = ReplaceInsensitive(input, "\\x58", "X");
            input = ReplaceInsensitive(input, "\\x59", "Y");
            input = ReplaceInsensitive(input, "\\x5A", "Z");
            input = ReplaceInsensitive(input, "\\x5B", "[");
            input = ReplaceInsensitive(input, "\\x5C", "\\");
            input = ReplaceInsensitive(input, "\\x5D", "]");
            input = ReplaceInsensitive(input, "\\x5E", "^");
            input = ReplaceInsensitive(input, "\\x5F", "_");
            input = ReplaceInsensitive(input, "\\x60", "`");
            input = ReplaceInsensitive(input, "\\x61", "a");
            input = ReplaceInsensitive(input, "\\x62", "b");
            input = ReplaceInsensitive(input, "\\x63", "c");
            input = ReplaceInsensitive(input, "\\x64", "d");
            input = ReplaceInsensitive(input, "\\x65", "e");
            input = ReplaceInsensitive(input, "\\x66", "f");
            input = ReplaceInsensitive(input, "\\x67", "g");
            input = ReplaceInsensitive(input, "\\x68", "h");
            input = ReplaceInsensitive(input, "\\x69", "i");
            input = ReplaceInsensitive(input, "\\x6A", "j");
            input = ReplaceInsensitive(input, "\\x6B", "k");
            input = ReplaceInsensitive(input, "\\x6C", "l");
            input = ReplaceInsensitive(input, "\\x6D", "m");
            input = ReplaceInsensitive(input, "\\x6E", "n");
            input = ReplaceInsensitive(input, "\\x6F", "o");
            input = ReplaceInsensitive(input, "\\x70", "p");
            input = ReplaceInsensitive(input, "\\x71", "q");
            input = ReplaceInsensitive(input, "\\x72", "r");
            input = ReplaceInsensitive(input, "\\x73", "s");
            input = ReplaceInsensitive(input, "\\x74", "t");
            input = ReplaceInsensitive(input, "\\x75", "u");
            input = ReplaceInsensitive(input, "\\x76", "v");
            input = ReplaceInsensitive(input, "\\x77", "w");
            input = ReplaceInsensitive(input, "\\x78", "x");
            input = ReplaceInsensitive(input, "\\x79", "y");
            input = ReplaceInsensitive(input, "\\x7A", "z");
            input = ReplaceInsensitive(input, "\\x7B", "{");
            input = ReplaceInsensitive(input, "\\x7C", "|");
            input = ReplaceInsensitive(input, "\\x7D", "}");
            input = ReplaceInsensitive(input, "\\x7E", "~");
            return input;
        }

        public static string ReplaceInsensitive(string str, string from, string to)
        {
            return str.Replace(from, to);
        }

        //public static string ReplaceInsensitive(string str, string old, string @new)
        //{
        //    @new = @new ?? "";
        //    if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(old) || old.Equals(@new, StringComparison.CurrentCultureIgnoreCase))
        //        return str;
        //    int foundAt = 0;
        //    while ((foundAt = str.IndexOf(old, foundAt, StringComparison.CurrentCultureIgnoreCase)) != -1)
        //    {
        //        str = str.Remove(foundAt, old.Length).Insert(foundAt, @new);
        //        foundAt += @new.Length;
        //    }
        //    return str;
        //}

        public static string FormatMoney(int value)
        {
            if (value == 0)
                return "0";
            return string.Format("{0:#,###}", value);
        }

        public static int Float2Int(float value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            return BitConverter.ToInt32(buffer, 0);
        }

        public static int[] ToArr(int address, int[] offset)
        {
            int[] newoffset = new int[offset.Length + 1];
            newoffset[0] = address;
            offset.CopyTo(newoffset, 1);
            return newoffset;
        }

        public static int Percent(int min, int max)
        {
            if (max == 0) return 0;
            int percent = (int)(min * 100 / max);
            if (percent == 0 && min > 0) return 1;
            if (percent >= 100) return 99;
            return percent;
        }

        public static int Bool2Int(bool value)
        {
            if (value == true)
                return 1;
            return 0;
        }

        public static string String2Hex(string s)
        {
            string hex = "";
            for (int i = 0; i < s.Length; i++)
            {
                hex += ConverterEx.Char2Int(s[i]).ToString("X2");
            }
            return hex;
        }

        public static string ReverseString(string s)
        {
            String temp = "";
            if (s.Length % 2 != 0)
            {
                s = s + "0";
            }
            for (int i = s.Length / 2 - 1; i >= 0; i--)
            {
                temp += s.Substring(i * 2, 2);
            }

            return temp;
        }

        public static int Char2Int(char c)
        {
            return (int)c;
        }

        public static int Hex2Int(string hex)
        {
            if (hex.Contains("?"))
                return -1;
            if (hex.Contains("#"))
            {
                return 257;                
            }
            int result = -1;
            int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
            return result;
        }

        public static int[] Hex2IntArr(string hex)
        {
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex = hex + "0";
            }
            int[] buff = new int[hex.Length / 2];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Hex2Int(hex.Substring(i * 2, 2));
            }
            return buff;
        }

        public static string Arr2String(int[] arr)
        {
            string output = "";
            for(int i = 0; i < arr.Length; i++)
            {
                output += arr[i].ToString() + " ";
            }
            return output + output.Length;
        }

        public static string Arr2String(byte[] arr)
        {
            string output = "";
            for (int i = 0; i < arr.Length; i++)
            {
                output += arr[i].ToString() + " ";
            }
            return output + output.Length;
        }

        public static string VISCII2Unicode(byte[] input)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in input)
            {
                if (c == '\0')
                    break;
                if (c < 256)
                    builder.Append(Unicodes[c]);
                else
                    builder.Append(c);
            }
            return builder.ToString();
        }

        public static string VISCII2UnicodeEx(byte[] input)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in input)
            {
                if (c == '\0')
                    continue;
                if (c < 256)
                    builder.Append(Unicodes[c]);
                else
                    builder.Append("?");
            }
            return builder.ToString();
        }

        public static string Unicode2VISCII(string input)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] >= 255)                
                    sb.Append(Unicode2VISCII(input[i]));
                else
                    sb.Append(input[i]);
            }
            return sb.ToString();
        }

       

        public static char Unicode2VISCII(char c)
        {
            for(int i = 0; i < 256; i++)
            {
                if (Unicodes[i] == c)
                    return (char)i;
            }
            return '?';
        }


        public static readonly char[] Unicodes = new char[]
        {
            '\u0000', '\u0001', '\u1EB2', '\u0003', '\u0004', '\u1EB4', '\u1EAA', '\u0007',
            '\u0008', '\u0009', '\u000A', '\u000B', '\u000C', '\u000D', '\u000E', '\u000F',
            '\u0010', '\u0011', '\u0012', '\u0013', '\u1EF6', '\u0015', '\u0016', '\u0017',
            '\u0018', '\u1EF8', '\u001A', '\u001B', '\u001C', '\u001D', '\u1EF4', '\u001F',
            '\u0020', '\u0021', '\u0022', '\u0023', '\u0024', '\u0025', '\u0026', '\u0027',
            '\u0028', '\u0029', '\u002A', '\u002B', '\u002C', '\u002D', '\u002E', '\u002F',
            '\u0030', '\u0031', '\u0032', '\u0033', '\u0034', '\u0035', '\u0036', '\u0037',
            '\u0038', '\u0039', '\u003A', '\u003B', '\u003C', '\u003D', '\u003E', '\u003F',
            '\u0040', '\u0041', '\u0042', '\u0043', '\u0044', '\u0045', '\u0046', '\u0047',
            '\u0048', '\u0049', '\u004A', '\u004B', '\u004C', '\u004D', '\u004E', '\u004F',
            '\u0050', '\u0051', '\u0052', '\u0053', '\u0054', '\u0055', '\u0056', '\u0057',
            '\u0058', '\u0059', '\u005A', '\u005B', '\u005C', '\u005D', '\u005E', '\u005F',
            '\u0060', '\u0061', '\u0062', '\u0063', '\u0064', '\u0065', '\u0066', '\u0067',
            '\u0068', '\u0069', '\u006A', '\u006B', '\u006C', '\u006D', '\u006E', '\u006F',
            '\u0070', '\u0071', '\u0072', '\u0073', '\u0074', '\u0075', '\u0076', '\u0077',
            '\u0078', '\u0079', '\u007A', '\u007B', '\u007C', '\u007D', '\u007E', '\u007F',
            '\u1EA0', '\u1EAE', '\u1EB0', '\u1EB6', '\u1EA4', '\u1EA6', '\u1EA8', '\u1EAC',
            '\u1EBC', '\u1EB8', '\u1EBE', '\u1EC0', '\u1EC2', '\u1EC4', '\u1EC6', '\u1ED0',
            '\u1ED2', '\u1ED4', '\u1ED6', '\u1ED8', '\u1EE2', '\u1EDA', '\u1EDC', '\u1EDE',
            '\u1ECA', '\u1ECE', '\u1ECC', '\u1EC8', '\u1EE6', '\u0168', '\u1EE4', '\u1EF2',
            '\u00D5', '\u1EAF', '\u1EB1', '\u1EB7', '\u1EA5', '\u1EA7', '\u1EA9', '\u1EAD',
            '\u1EBD', '\u1EB9', '\u1EBF', '\u1EC1', '\u1EC3', '\u1EC5', '\u1EC7', '\u1ED1',
            '\u1ED3', '\u1ED5', '\u1ED7', '\u1EE0', '\u01A0', '\u1ED9', '\u1EDD', '\u1EDF',
            '\u1ECB', '\u1EF0', '\u1EE8', '\u1EEA', '\u1EEC', '\u01A1', '\u1EDB', '\u01AF',
            '\u00C0', '\u00C1', '\u00C2', '\u00C3', '\u1EA2', '\u0102', '\u1EB3', '\u1EB5',
            '\u00C8', '\u00C9', '\u00CA', '\u1EBA', '\u00CC', '\u00CD', '\u0128', '\u1EF3',
            '\u0110', '\u1EE9', '\u00D2', '\u00D3', '\u00D4', '\u1EA1', '\u1EF7', '\u1EEB',
            '\u1EED', '\u00D9', '\u00DA', '\u1EF9', '\u1EF5', '\u00DD', '\u1EE1', '\u01B0',
            '\u00E0', '\u00E1', '\u00E2', '\u00E3', '\u1EA3', '\u0103', '\u1EEF', '\u1EAB',
            '\u00E8', '\u00E9', '\u00EA', '\u1EBB', '\u00EC', '\u00ED', '\u0129', '\u1EC9',
            '\u0111', '\u1EF1', '\u00F2', '\u00F3', '\u00F4', '\u00F5', '\u1ECF', '\u1ECD',
            '\u1EE5', '\u00F9', '\u00FA', '\u0169', '\u1EE7', '\u00FD', '\u1EE3', '\u1EEE',
        };

        public static readonly string[] UnicodesStr = new string[]
       {
            @"\u0000", @"\u0001", @"\u1EB2", @"\u0003", @"\u0004", @"\u1EB4", @"\u1EAA", @"\u0007",
            @"\u0008", @"\u0009", @"\u000A", @"\u000B", @"\u000C", @"\u000D", @"\u000E", @"\u000F",
            @"\u0010", @"\u0011", @"\u0012", @"\u0013", @"\u1EF6", @"\u0015", @"\u0016", @"\u0017",
            @"\u0018", @"\u1EF8", @"\u001A", @"\u001B", @"\u001C", @"\u001D", @"\u1EF4", @"\u001F",
            @"\u0020", @"\u0021", @"\u0022", @"\u0023", @"\u0024", @"\u0025", @"\u0026", @"\u0027",
            @"\u0028", @"\u0029", @"\u002A", @"\u002B", @"\u002C", @"\u002D", @"\u002E", @"\u002F",
            @"\u0030", @"\u0031", @"\u0032", @"\u0033", @"\u0034", @"\u0035", @"\u0036", @"\u0037",
            @"\u0038", @"\u0039", @"\u003A", @"\u003B", @"\u003C", @"\u003D", @"\u003E", @"\u003F",
            @"\u0040", @"\u0041", @"\u0042", @"\u0043", @"\u0044", @"\u0045", @"\u0046", @"\u0047",
            @"\u0048", @"\u0049", @"\u004A", @"\u004B", @"\u004C", @"\u004D", @"\u004E", @"\u004F",
            @"\u0050", @"\u0051", @"\u0052", @"\u0053", @"\u0054", @"\u0055", @"\u0056", @"\u0057",
            @"\u0058", @"\u0059", @"\u005A", @"\u005B", @"\u005C", @"\u005D", @"\u005E", @"\u005F",
            @"\u0060", @"\u0061", @"\u0062", @"\u0063", @"\u0064", @"\u0065", @"\u0066", @"\u0067",
            @"\u0068", @"\u0069", @"\u006A", @"\u006B", @"\u006C", @"\u006D", @"\u006E", @"\u006F",
            @"\u0070", @"\u0071", @"\u0072", @"\u0073", @"\u0074", @"\u0075", @"\u0076", @"\u0077",
            @"\u0078", @"\u0079", @"\u007A", @"\u007B", @"\u007C", @"\u007D", @"\u007E", @"\u007F",
            @"\u1EA0", @"\u1EAE", @"\u1EB0", @"\u1EB6", @"\u1EA4", @"\u1EA6", @"\u1EA8", @"\u1EAC",
            @"\u1EBC", @"\u1EB8", @"\u1EBE", @"\u1EC0", @"\u1EC2", @"\u1EC4", @"\u1EC6", @"\u1ED0",
            @"\u1ED2", @"\u1ED4", @"\u1ED6", @"\u1ED8", @"\u1EE2", @"\u1EDA", @"\u1EDC", @"\u1EDE",
            @"\u1ECA", @"\u1ECE", @"\u1ECC", @"\u1EC8", @"\u1EE6", @"\u0168", @"\u1EE4", @"\u1EF2",
            @"\u00D5", @"\u1EAF", @"\u1EB1", @"\u1EB7", @"\u1EA5", @"\u1EA7", @"\u1EA9", @"\u1EAD",
            @"\u1EBD", @"\u1EB9", @"\u1EBF", @"\u1EC1", @"\u1EC3", @"\u1EC5", @"\u1EC7", @"\u1ED1",
            @"\u1ED3", @"\u1ED5", @"\u1ED7", @"\u1EE0", @"\u01A0", @"\u1ED9", @"\u1EDD", @"\u1EDF",
            @"\u1ECB", @"\u1EF0", @"\u1EE8", @"\u1EEA", @"\u1EEC", @"\u01A1", @"\u1EDB", @"\u01AF",
            @"\u00C0", @"\u00C1", @"\u00C2", @"\u00C3", @"\u1EA2", @"\u0102", @"\u1EB3", @"\u1EB5",
            @"\u00C8", @"\u00C9", @"\u00CA", @"\u1EBA", @"\u00CC", @"\u00CD", @"\u0128", @"\u1EF3",
            @"\u0110", @"\u1EE9", @"\u00D2", @"\u00D3", @"\u00D4", @"\u1EA1", @"\u1EF7", @"\u1EEB",
            @"\u1EED", @"\u00D9", @"\u00DA", @"\u1EF9", @"\u1EF5", @"\u00DD", @"\u1EE1", @"\u01B0",
            @"\u00E0", @"\u00E1", @"\u00E2", @"\u00E3", @"\u1EA3", @"\u0103", @"\u1EEF", @"\u1EAB",
            @"\u00E8", @"\u00E9", @"\u00EA", @"\u1EBB", @"\u00EC", @"\u00ED", @"\u0129", @"\u1EC9",
            @"\u0111", @"\u1EF1", @"\u00F2", @"\u00F3", @"\u00F4", @"\u00F5", @"\u1ECF", @"\u1ECD",
            @"\u1EE5", @"\u00F9", @"\u00FA", @"\u0169", @"\u1EE7", @"\u00FD", @"\u1EE3", @"\u1EEE",
       };
    }
}
