const InfiniteScroll = (function (actionUrl, container) {
    let url = `${actionUrl.replace("&amp;", "&")}page=`;
    let nextPage = 2;
    let ajaxIsRunning = false;
    let itemsEnded = false;
    const loader = "#loader";

    function getData() {
        ajaxIsRunning = true;

        $.ajax({
            method: "GET",
            url: url + nextPage,
            dataType: "html",
            beforeSend: () => {
                $(loader).removeClass("d-none");
            },
            complete: () => {
                $(loader).addClass("d-none");
            },
            success: (result) => {
                let loadedItems = $(container, $(result)).children();
                itemsEnded = loadedItems.length === 0;
                
                if (!itemsEnded) {
                    nextPage++;
                    render(loadedItems);
                }
            }
        }).always(() => {
            ajaxIsRunning = false;
        });
    }

    function render(res) {
        $(container).append(res);
    }

    return {
        init: function () {
            window.onscroll = () => {
                if ((window.innerHeight + window.scrollY) >= $(container).outerHeight()
                    && !itemsEnded && !ajaxIsRunning) {
                    
                    ajaxIsRunning = true;
                    getData();
                }
            }
        }
    }
});

const Authorization = (function () {
    const formElem = ".auth-form";

    return {
        init: function () {
            $(formElem).each((i, elem) => {
                $(".form-auth-toggle", elem).on("click", () => {
                    $(formElem).toggleClass("auth-form-active").toggleClass("auth-form-hidden");
                })
            });
        }
    }
}());

const Settings = (function () {
    const elem = "button[data-target-name]";

    function toggleSettingsItem(settingsItem, form) {
        $(settingsItem).toggleClass("d-none");
        $(form).toggleClass("d-none");
        $(form).toggleClass("settings-item-active");
    }

    return {
        init: function () {
            $(elem).on("click", e => {
                let formName = $(e.target).attr("data-target-name");
                let form = $(`form[name="${formName}"]`);

                $(".settings-item-active").each((i, elem) => $(".settings-item-close", elem).click());

                let parent = $(e.target).closest(".settings-item");
                toggleSettingsItem(parent, form);

                $(".settings-item-close", form).one("click", event => {
                    event.preventDefault();
                    toggleSettingsItem(parent, form);
                })
            });


        }
    }
}());

let Post = (function (rateActionUrl) {
    let container = ".posts";
    let elem = ".post";

    function ratePost(post, isLike) {
        let ratingCounter = $(".rating-counter", post);
        let downRateBtn = $(".down-rating-btn", post);
        let upRateBtn = $(".up-rating-btn", post);

        if (isLike) {
            rateAjax(post.dataset.postId, true, () => {
                if ($(upRateBtn).hasClass("up-rating-active") === false)
                    $(ratingCounter).text(Number($(ratingCounter).text()) + 1);

                if ($(downRateBtn).hasClass("down-rating-active"))
                    $(downRateBtn).removeClass("down-rating-active")
                else
                    $(upRateBtn).addClass("up-rating-active")
            });
        } else {
            rateAjax(post.dataset.postId, false, () => {
                if ($(downRateBtn).hasClass("down-rating-active") === false)
                    $(ratingCounter).text(Number($(ratingCounter).text()) - 1);

                if ($(upRateBtn).hasClass("up-rating-active"))
                    $(upRateBtn).removeClass("up-rating-active")
                else
                    $(downRateBtn).addClass("down-rating-active")
            });
        }
    }

    function rateAjax(postId, isLike, successFunc) {
        $.ajax({
            method: "POST",
            url: rateActionUrl,
            data: {postId: postId, isLike: isLike},
            success: successFunc()
        });
    }

    return {
        init: function () {
            $(container).on("click", elem, (e) => {
                let target = e.target;
                let postElem = e.currentTarget;

                if ($(target).hasClass("up-rating-btn"))
                    ratePost(postElem, true);
                else if ($(target).hasClass("down-rating-btn"))
                    ratePost(postElem, false);
            });
        }
    }
});

const Comments = (function (getCommentRepliesUrl) {
    function branchToggle(comment) {
        let btnShowBranchElem = $(".btn-show-branch:first", comment)
        let btnHideBranchElem = $(".btn-hide-branch:first", comment);
        let commentRepliesElem = $(".comment-replies-branch", comment)

        if ($(commentRepliesElem).children().length === 0) {
            $.ajax({
                method: "POST",
                url: getCommentRepliesUrl,
                data: {commentId: $(comment).data("comment-id")},
                dataType: "html",
                success: (result) => $(commentRepliesElem).prepend($(result))
            });
        }

        $(btnShowBranchElem).toggleClass("d-none");
        $(btnHideBranchElem).toggleClass("d-none");
        $(commentRepliesElem).toggleClass("d-none");
    }

    function replyToggle(comment) {
        let replyInput = $(".reply-comment-form:first", comment);

        $(".reply-comment-form").not(replyInput).addClass("d-none");
        $(replyInput).toggleClass("d-none");
    }

    function sendReply(comment) {
        let form = $(".reply-comment-form:first", comment);
        let commentRepliesElem = $(".comment-replies-branch:first", comment)

        $(form).one("submit", e => {
            e.preventDefault();

            sendCommentAjax(form, (res) => {
                if ($(commentRepliesElem).hasClass("d-none") === false || $(commentRepliesElem).children().length > 0)
                    $(commentRepliesElem).append($(res))
            });
        });
    }

    function sendCommentAjax(form, successFunc) {
        $.ajax({
            method: $(form).attr("method"),
            url: $(form).attr("action"),
            data: $(form).serialize(),
            dataType: "html",
            success: (result) => {
                $(".comment-input", form).val("");
                successFunc(result);
            }
        });
    }
    
    return {
        init: () => {
            $("#newCommentForm").submit((e) => {
                e.preventDefault();

                let form = e.target;
                sendCommentAjax(form, (result) => {
                    $(".comments").prepend(result);
                    $(".no-comments").addClass("d-none");
                });
            });

            $(".comments").on("click", ".comment", (e) => {
                let target = e.target;
                let commentElem = $(target).closest(".comment");

                if ($(target).hasClass("reply-btn"))
                    replyToggle(commentElem);
                else if ($(target).hasClass("btn-branch"))
                    branchToggle(commentElem);
                else if ($(target).is(":submit"))
                    sendReply(commentElem)

                e.stopPropagation();
            })
        }
    }
});

//remote validation not on every key press
$("input[data-val-remote]").on("keyup", () => false);

function autosize(elem) {
    let maxHeight = Number($(elem).css("max-height").replace(/px$/, ''));

    if (elem.scrollHeight > maxHeight) {
        elem.style.maxHeight = maxHeight + "px"
        elem.style.overflow = "auto";
    } else {
        elem.style.height = "auto";
        elem.style.overflow = "hidden";
        elem.style.height = (elem.scrollHeight) + "px";
    }
}