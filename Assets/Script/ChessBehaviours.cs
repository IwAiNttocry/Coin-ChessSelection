using UnityEngine;
using UnityEngine.EventSystems;

public class SelectChess : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private IChessState _currentState;
    public IChessState NextState { get; private set; }

    public interface IChessState
    {
        void OnEnter(SelectChess ctx);
        void OnUpdate(SelectChess ctx);
        void OnExit(SelectChess ctx);
    }

    private class IdleState : IChessState
    {
        private float _timer;

        public void OnEnter(SelectChess ctx) { }

        public void OnUpdate(SelectChess ctx)
        {
            _timer += Time.deltaTime;
            if (_timer < 0.5f) return;
            _timer = 0f;

            Transform hit = ctx.RaycastForPiece();
            if (hit != null)
                ctx.TransitionTo(new HoveredState(hit));
        }

        public void OnExit(SelectChess ctx) { }
    }

    private class HoveredState : IChessState
    {
        private readonly Transform _piece;
        private float _timer;

        public HoveredState(Transform piece) => _piece = piece;

        public void OnEnter(SelectChess ctx)
        {
            _piece.position += new Vector3(0, 0.5f, 0);
        }

        public void OnUpdate(SelectChess ctx)
        {
            if (ctx.IsClicked())
            {
                ctx.TransitionTo(new SelectedState(_piece));
                return;
            }

            _timer += Time.deltaTime;
            if (_timer < 0.5f) return;
            _timer = 0f;

            Transform hit = ctx.RaycastForPiece();
            bool stillHere = hit == _piece || ctx.IsMouseNearPiece(_piece);
            if (!stillHere)
                ctx.TransitionTo(new IdleState());
        }

        public void OnExit(SelectChess ctx)
        {
            if (ctx.NextState is not SelectedState)
                _piece.position -= new Vector3(0, 0.5f, 0);
        }
    }

    private class SelectedState : IChessState
    {
        private readonly Transform _piece;

        public SelectedState(Transform piece) => _piece = piece;

        public void OnEnter(SelectChess ctx) { }

        public void OnUpdate(SelectChess ctx)
        {
            if (!ctx.IsClicked()) return;
            ctx.TransitionTo(new IdleState());
        }

        public void OnExit(SelectChess ctx)
        {
            _piece.position -= new Vector3(0, 0.5f, 0);
        }
    }

    void Start() => TransitionTo(new IdleState());

    void Update() => _currentState?.OnUpdate(this);

    public void TransitionTo(IChessState newState)
    {
        NextState = newState;
        _currentState?.OnExit(this);
        _currentState = newState;
        NextState = null;
        _currentState.OnEnter(this);
    }

    public Transform RaycastForPiece()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, Mathf.Infinity))
            if (hit.transform.CompareTag("ChessPiece"))
                return hit.transform;
        return null;
    }

    public bool IsMouseNearPiece(Transform piece)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(piece.position);
        return Vector2.Distance(Input.mousePosition, screenPos) < 60f;
    }

    public bool IsClicked()
    {
        return Input.GetMouseButtonDown(0)
            && !EventSystem.current.IsPointerOverGameObject();
    }
}