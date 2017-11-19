using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class SwipeGesture : MonoBehaviour {
    private float thresholdSecond = 1.0f;
    private float thresholdDistance = 100.0f;
    private DateTime starttime;
    private Vector2 startPosition;

    private Subject<Unit> onSwipeLeft = new Subject<Unit>();
    public UniRx.IObservable<Unit> OnSwipeLeft => onSwipeLeft;

    private Subject<Unit> onSwipeRight = new Subject<Unit>();
    public UniRx.IObservable<Unit> OnSwipeRight => onSwipeRight;

    private Subject<Unit> onSwipeDown = new Subject<Unit>();
    public UniRx.IObservable<Unit> OnSwipeDown => onSwipeDown;

    private Subject<Unit> onSwipeUp = new Subject<Unit>();
    public UniRx.IObservable<Unit> OnSwipeUp => onSwipeUp;


    void OnEnable() {
        var eventTrigger = gameObject.AddComponent<ObservableEventTrigger>();

        eventTrigger.OnBeginDragAsObservable()
            .TakeUntilDisable(this)
            .Where(eventData => eventData.pointerDrag.gameObject == gameObject)
            .Select(eventData => eventData.position)
            .Subscribe(position => {
                startPosition = position;
                starttime = DateTime.Now;
            });

        var onEndDragObservable = eventTrigger
            .OnEndDragAsObservable()
            .TakeUntilDisable(this)
            .Where(eventData => (DateTime.Now - starttime).TotalSeconds < thresholdSecond)
            .Select(eventData => eventData.position)
            .Share();

        //左
        onEndDragObservable
            .Where(position => startPosition.x > position.x)
            .Where(position => Mathf.Abs(startPosition.x - position.x) >= thresholdDistance)
            .Subscribe(_ => { onSwipeLeft.OnNext(Unit.Default); });

        //右
        onEndDragObservable
            .Where(position => position.x > startPosition.x)
            .Where(position => Mathf.Abs(position.x - startPosition.x) >= thresholdDistance)
            .Subscribe(_ => { onSwipeRight.OnNext(Unit.Default); });

        //下
        onEndDragObservable
            .Where(position => startPosition.y > position.y)
            .Where(position => Mathf.Abs(startPosition.y - position.y) >= thresholdDistance)
            .Subscribe(_ => { onSwipeDown.OnNext(Unit.Default); });

        //上
        onEndDragObservable
            .Where(position => position.y > startPosition.y)
            .Where(position => Mathf.Abs(position.y - startPosition.y) >= thresholdDistance)
            .Subscribe(_ => { onSwipeUp.OnNext(Unit.Default); });
    }
}